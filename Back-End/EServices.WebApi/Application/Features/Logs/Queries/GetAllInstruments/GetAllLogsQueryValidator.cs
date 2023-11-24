


using System.Linq;
using Application.Parameters;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Instruments.Queries.GetAllInstruments
{
    public class GetAllLogsQueryValidator : RequestParamererValidator<GetAllLogsQuery>
    {
            public GetAllLogsQueryValidator(){
            
            RuleFor(e => e.Filter)
            .Must(ValidateFieldsFromQueryString)
            .When(e => !string.IsNullOrEmpty(e.Filter))
            .WithMessage("Filter Field is invalid");
            // valid filter format - field:operator:value

            RuleFor(e => e.Sort)
             .Must(ValidateFieldsFromQueryString)
             .When(e => !string.IsNullOrEmpty(e.Sort))
             .WithMessage("Sort Field is invalid");
        }
        private bool ValidateFieldsFromQueryString(string input)
        {
            bool isValid = input.Contains(',') ? !input.Split(',').ToList().Any(e => ValidateFields(e.Split(':')[0]) == false)
                               : ValidateFields(input.Split(':')[0]);
            return isValid;
        }
        private bool ValidateFields(string fieldName)
        {
            return new Log().GetType().GetProperties().Any(e => string.Equals(e.Name, fieldName, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}