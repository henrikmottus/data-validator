using DataValidator.DTOs;
using DataValidator.Services;

namespace DataValidator.Tests
{
    public class CardNumberTests
    {
        [Theory]
        [InlineData(379763718409150)]
        [InlineData(348743990217230)]
        [InlineData(348144897532633)]
        [InlineData(373139866482081)]
        [InlineData(347232030039318)]
        [InlineData(346835109209975)]
        [InlineData(346725264257457)]
        [InlineData(377990468338949)]
        [InlineData(374343840009767)]
        public void AmexNumber_ShouldReturnAmexType(long? number)
        {
            var response = new ValidationResponse();
            response = ValidatorService.ValidateNumber(number, response);

            Assert.Equal(CardType.AmericanExpress, response.CardType);
            Assert.Empty(response.Errors);
        }

        [Theory]
        [InlineData(4166315287009629)]
        [InlineData(4587639874779547)]
        [InlineData(4485166425537664)]
        [InlineData(4485283437291618)]
        [InlineData(4396635509443773)]
        [InlineData(4865824332579373)]
        [InlineData(4353948125557459)]
        [InlineData(4697808804758791)]
        [InlineData(4576508311965899)]
        [InlineData(4121380742809)]
        public void VisaNumber_ShouldReturnVisaType(long? number)
        {
            var response = new ValidationResponse();
            response = ValidatorService.ValidateNumber(number, response);

            Assert.Equal(CardType.Visa, response.CardType);
            Assert.Empty(response.Errors);
        }

        [Theory]
        [InlineData(5377185975221501)]
        [InlineData(5461806357796540)]
        [InlineData(2720512348103523)]
        [InlineData(5498193690513875)]
        [InlineData(5338158621932845)]
        [InlineData(5486196587938125)]
        [InlineData(5566264895387550)]
        [InlineData(2354038269175624)]
        [InlineData(2610934675602310)]
        [InlineData(2221965128403857)]
        public void MasterCardNumber_ShouldReturnMasterCardType(long? number)
        {
            var response = new ValidationResponse();
            response = ValidatorService.ValidateNumber(number, response);

            Assert.Equal(CardType.MasterCard, response.CardType);
            Assert.Empty(response.Errors);
        }

        [Theory]
        [InlineData(37976371840915)]
        [InlineData(3487439902172309)]
        [InlineData(348144897532665)]
        [InlineData(5131398664820)]
        [InlineData(5631398664820235)]
        [InlineData(5031398664820235)]
        [InlineData(2721398664820232)]
        [InlineData(2220398664820235)]
        [InlineData(45723203003931802)]
        [InlineData(4468351092099)]
        public void InvalidNumber_ShouldReturnErrors(long? number)
        {
            var response = new ValidationResponse();
            response = ValidatorService.ValidateNumber(number, response);

            Assert.NotEmpty(response.Errors);
        }
    }
}