using DataValidator.DTOs;
using DataValidator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidator.Tests
{
    public class CardCvcTests
    {
        [Theory]
        [InlineData(123, CardType.Visa)]
        [InlineData(455, CardType.Visa)]
        [InlineData(664, CardType.MasterCard)]
        [InlineData(111, CardType.MasterCard)]
        public void CvcWith3DigitsAndCardTypeVisaOrMasterCard_ShouldBeValid(int? cvc, CardType cardType)
        {
            var response = new ValidationResponse { CardType = cardType};
            response = ValidatorService.ValidateCVC(cvc, response);


            Assert.Empty(response.Errors);
        }

        [Theory]
        [InlineData(1230)]
        [InlineData(4551)]
        [InlineData(6642)]
        [InlineData(1113)]
        public void CvcWith4DigitsAndCardTypeAmericanExpress_ShouldBeValid(int? cvc)
        {
            var response = new ValidationResponse { CardType = CardType.AmericanExpress };
            response = ValidatorService.ValidateCVC(cvc, response);


            Assert.Empty(response.Errors);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(321)]
        [InlineData(6642)]
        [InlineData(1113)]
        public void CvcWith3Or4DigitsAndUnknownCardType_ShouldBeValid(int? cvc)
        {
            var response = new ValidationResponse { CardType = CardType.Unknown };
            response = ValidatorService.ValidateCVC(cvc, response);


            Assert.Empty(response.Errors);
        }

        [Fact]
        public void NullCvc_ShouldBeInvalid()
        {
            var response = new ValidationResponse();
            response = ValidatorService.ValidateCVC(null, response);


            Assert.NotEmpty(response.Errors);
        }

        [Theory]
        [InlineData(12, CardType.Visa)]
        [InlineData(4556, CardType.Visa)]
        [InlineData(66, CardType.MasterCard)]
        [InlineData(1110, CardType.MasterCard)]
        [InlineData(682, CardType.AmericanExpress)]
        [InlineData(97315, CardType.AmericanExpress)]
        [InlineData(1, CardType.Unknown)]
        [InlineData(10000, CardType.Unknown)]
        public void IncorrectCvcAndUnknownCardTypeCombination_ShouldBeInvalid(int? cvc, CardType cardType)
        {
            var response = new ValidationResponse { CardType = cardType };
            response = ValidatorService.ValidateCVC(cvc, response);


            Assert.NotEmpty(response.Errors);
        }
    }
}
