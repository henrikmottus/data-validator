using DataValidator.DTOs;

namespace DataValidator.Services
{
    public static class ValidatorService
    {
        public static ValidationResponse ValidateCVC(int? cvc, ValidationResponse response)
        {
            if (cvc is null)
            {
                response.Errors.Add("E003", "Card CVC not provided!");
                return response;
            }

            if (cvc is not null)
            {
                if (IsValidCvcForAnyCard(cvc, response))
                {
                    response.Errors.Add("E013", "Card CVC is invalid!");
                }
                else if (IsvalidCvcForVisaOrMasterCard(cvc, response))
                {
                    response.Errors.Add("E014", "Card CVC is invalid For Visa or MasterCard!");
                }
                else if (IsValidCvcForAmericanExpress(cvc, response))
                {
                    response.Errors.Add("E018", "Card CVC is invalid for American Express!");
                }
            }

            return response;
        }

        private static bool IsValidCvcForAnyCard(int? cvc, ValidationResponse response)
        {
            return (cvc < 100 || cvc >= 10000) && response.CardType == CardType.Unknown;
        }

        private static bool IsValidCvcForAmericanExpress(int? cvc, ValidationResponse response)
        {
            return (cvc < 1000 || cvc >= 10000) && response.CardType == CardType.AmericanExpress;
        }

        private static bool IsvalidCvcForVisaOrMasterCard(int? cvc, ValidationResponse response)
        {
            return (cvc < 100 || cvc >= 1000) && (response.CardType == CardType.Visa || response.CardType == CardType.MasterCard);
        }

        public static ValidationResponse ValidateDate(string? issueDate, ValidationResponse response)
        {
            if (string.IsNullOrWhiteSpace(issueDate))
            {
                response.Errors.Add("E002", "Card issue date not provided!");
                return response;
            }

            // Split string by '/'
            var issueDateSplit = issueDate.Split('/');
            // If the created array doesn't have two elements (one for month, one for year), the input is invalid
            if (issueDateSplit.Length != 2)
            {
                response.Errors.Add("E011", "Card issue date format is incorrect!");
                return response;
            }
            var monthString = issueDateSplit[0];
            var wasMonthParsed = int.TryParse(monthString, out var month);
            // If month couldn't be parsed, the input is invalid
            if (!wasMonthParsed)
            {
                response.Errors.Add("E011", "Card issue date format is incorrect!");
                return response;
            }

            var yearString = issueDateSplit[1];
            if (yearString.Length == 2)
            {
                yearString = "20" + yearString;
            }

            var wasYearParsed = int.TryParse(yearString, out var year);
            // If year couldn't be parsed, the input is invalid
            if (!wasYearParsed)
            {
                response.Errors.Add("E011", "Card issue date format is incorrect!");
                return response;
            }

            // Year shouldn't be in the future
            if (year > DateTime.Today.Year)
            {
                response.Errors.Add("E016", "Issue date can't be in the future!");
            }

            //Month should be in the 1-12 range
            if (month < 1 || month > 12)
            {
                response.Errors.Add("E017", "Issue date month is invalid!");
            }

            // Assume the card expires 3 years after it was issued
            if (year + 3 < DateTime.Today.Year || (year + 3 == DateTime.Today.Year && month < DateTime.Today.Month))
            {
                response.Errors.Add("E012", "Card has expired!");
            }

            return response;
        }


        public static ValidationResponse ValidateOwner(string? owner, ValidationResponse response)
        {
            if (string.IsNullOrWhiteSpace(owner))
            {
                response.Errors.Add("E001", "Card owner not provided!");
                return response;
            }

            foreach(var c in owner)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    if (c == owner.Last() && char.IsPunctuation(c))
                    {
                        continue;
                    }
                    response.Errors.Add("E010", "Card owner shouldn't include numbers and special characters!");
                    return response;
                }
            }

            return response;
        }

        public static ValidationResponse ValidateNumber(long? nullableNumber, ValidationResponse response)
        {
            if (nullableNumber is null)
            {
                response.Errors.Add("E004", "Card number not provided!");
                return response;
            }

            var number = (long)nullableNumber;

            var digits = new List<long>();
            while (number > 0)
            {
                digits.Add(number % 10);
                number /= 10;
            }

            response = SetCardType(digits, response);

            response = CheckCardNumber(digits, response);

            return response;
        }

        private static ValidationResponse CheckCardNumber(List<long> digits, ValidationResponse response)
        {
            // Luhn algorithm
            long result = 0;
            for (var i = 1; i < digits.Count(); i++)
            {
                var isParityIndex = i % 2 != 0;

                var num = digits[i];

                if (isParityIndex)
                {
                    var res = num * 2;
                    if (res >= 10)
                    {
                        var resString = res.ToString();
                        res = resString.Sum(c => int.Parse(c.ToString()));
                    }
                    result += res;
                }
                else
                {
                    result += num;
                }
            }

            if ((result + digits.First()) % 10 != 0)
            {
                response.Errors.Add("E019", "Card number is invalid!");
            }

            return response;
        }

        private static ValidationResponse SetCardType(List<long> digits, ValidationResponse response)
        {
            // Visa
            if (new[] { 13, 16 }.Contains(digits.Count) && digits.Last() == 4)
            {
                response.CardType = CardType.Visa;
            }
            // MasterCard
            else if (digits.Count == 16)
            {
                if (digits.Last() == 2)
                {
                    var startDigits = digits.TakeLast(4).Reverse();
                    long start = 0;
                    foreach (var digit in startDigits)
                    {
                        start = 10 * start + digit;
                    }
                    if (2221 > start || start > 2720)
                    {
                        response.Errors.Add("E015", "Card type couldn't be determined!");
                        return response;
                    }
                }
                else if (digits.Last() == 5)
                {
                    var startDigits = digits.TakeLast(2).Reverse();
                    long start = 0;
                    foreach (var digit in startDigits)
                    {
                        start = 10 * start + digit;
                    }
                    if (51 > start || start > 55)
                    {
                        response.Errors.Add("E015", "Card type couldn't be determined!");
                        return response;
                    }
                }
                else
                {
                    response.Errors.Add("E015", "Card type couldn't be determined!");
                    return response;
                }

                response.CardType = CardType.MasterCard;
            }
            // American Express
            else if (digits.Count == 15)
            {
                var startDigits = digits.TakeLast(2).Reverse();
                long start = 0;
                foreach (var digit in startDigits)
                {
                    start = 10 * start + digit;
                }

                if (start == 34 || start == 37)
                {
                    response.CardType = CardType.AmericanExpress;
                }
            }
            else
            {
                response.Errors.Add("E015", "Card type couldn't be determined!");
            }

            return response;
        }
    }
}
