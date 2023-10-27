
using System.Linq;
using Application.Filters;
using FluentValidation;

namespace Application.Parameters
{
    public class RequestParamererValidator<T> : AbstractValidator<T> where T : IRequestParameter
    {
        private string[] filterOperators = new string[] { "eq", "neq", "sw", "nsw", "ew", "new", "con", "ncon", "lt", "lte", "gt", "gte" };
        public RequestParamererValidator()
        {
            RuleFor(e => e.Filter)
            .Must(ValidateFilterString)
            .When(e => !string.IsNullOrEmpty(e.Filter))
            .WithMessage("Filter is invalid"); // valid filter format - field:operator:value
        }

        private bool ValidateFilterString(string input)
        {
            bool isValid = input.Contains(',') ? !input.Split(',').ToList().Any(e => validateColon(e) == false) : validateColon(input);
            return isValid;
        }

        private bool validateColon(string input)
        {
            return string.IsNullOrEmpty(input) ? false :
                        input.Contains(':') &&
                        input.Split(':').Length == 3 &&
                        filterOperators.Any(e => e == input.Split(':')[1].ToLower());
        }


    }
}