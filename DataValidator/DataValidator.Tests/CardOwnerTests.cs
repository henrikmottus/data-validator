using DataValidator.DTOs;
using DataValidator.Services;

namespace DataValidator.Tests
{
    public class CardOwnerTests
    {
        [Theory]
        [InlineData("Henrik Mõttus")]
        [InlineData("Kairi Kaur")]
        [InlineData("Gökçen Özder Öcal")]
        [InlineData("Dido")]
        [InlineData("Robert Downey Jr.")]
        public void LetteredOwner_ShouldReturnHaveNoErrors(string? owner)
        {
            var response = new ValidationResponse();
            response = ValidatorService.ValidateOwner(owner, response);

            Assert.Empty(response.Errors);
        }

        [Theory]
        [InlineData("!Henrik Mõttus")]
        [InlineData("Ka1r1 Kaur0")]
        [InlineData("/A*")]
        public void OwnerWithDigitsOrSpecialCharacters_ShouldReturnError(string? owner)
        {
            var response = new ValidationResponse();
            response = ValidatorService.ValidateOwner(owner, response);

            Assert.NotEmpty(response.Errors);
        }
    }
}