using DataValidator.DTOs;
using DataValidator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidator.Tests
{
    public class CardIssueDateTests
    {
        [Theory]
        [InlineData("12/20")]
        [InlineData("11/19")]
        [InlineData("10/2020")]
        [InlineData("12/2021")]
        [InlineData("12/2022")]
        public void IssueDateThatIsLessThan3YearsOld_ShouldBeValid(string? date)
        {
            var response = new ValidationResponse();
            response = ValidatorService.ValidateDate(date, response);


            Assert.Empty(response.Errors);
        }

        [Theory]
        [InlineData("2/12")]
        [InlineData("12")]
        [InlineData("12/1")]
        [InlineData("12/12")]
        [InlineData("13/20")]
        [InlineData("10/19")]
        [InlineData(null)]
        public void InvalidIssueDate_ShouldReturnError(string? date)
        {
            var response = new ValidationResponse();
            response = ValidatorService.ValidateDate(date, response);


            Assert.NotEmpty(response.Errors);
        }
    }
}
