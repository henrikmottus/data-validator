using DataValidator.DTOs;
using DataValidator.Services;

namespace DataValidator.Tests
{
    public class CardOwnerTests
    {
        [Theory]
        [InlineData("Henrik M�ttus")]
        [InlineData("Kairi Kaur")]
        [InlineData("G�k�en �zder �cal")]
        [InlineData("Dido")]
        [InlineData("Robert Downey Jr.")]
        public void LetteredOwner_ShouldReturnHaveNoErrors(string? owner)
        {
            var response = new ValidationResponse();
            response = ValidatorService.ValidateOwner(owner, response);

            Assert.Empty(response.Errors);
        }

        [Theory]
        [InlineData("!Henrik M�ttus")]
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