using DataValidator.DTOs;
using DataValidator.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataValidator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidatorController : ControllerBase
    {
        [HttpPost]
        public ValidationResponse Validate([FromBody] CardDataDto? cardData)
        {
            if (cardData is null)
            {
                return new ValidationResponse
                {
                    Errors = new Dictionary<string, string>
                    {
                        { "E000", "No data provided" }
                    }
                };
            }

            var response = new ValidationResponse();


            response = ValidatorService.ValidateNumber(cardData.Number, response);
            response = ValidatorService.ValidateOwner(cardData.Owner, response);
            response = ValidatorService.ValidateDate(cardData.IssueDate, response);
            response = ValidatorService.ValidateCVC(cardData.CVC, response);

            return response;
        }
    }
}
