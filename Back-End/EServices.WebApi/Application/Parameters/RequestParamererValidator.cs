
using System.Linq;
using Application.Filters;
using FluentValidation;

namespace Application.Parameters
{
    public class RequestParamererValidator<T> : AbstractValidator<T> where T : IRequestParameter
    {
        private string[] filterOperators = new string[] { "eq", "neq", "sw", "nsw", "ew", "new", "con", "ncon", "lt", "lte", "gt", "gte" };
        private string[] sortDirection = new string[] { "asc", "desc" };

        public RequestParamererValidator()
        {
            RuleFor(e => e.Filter)
            .Must(ValidateFilterString)
            .When(e => !string.IsNullOrEmpty(e.Filter))
            .WithMessage("Filter is invalid"); // valid filter format - field:operator:value

            RuleFor(e => e.Sort)
             .Must(ValidateSortString)
             .When(e => !string.IsNullOrEmpty(e.Sort))
             .WithMessage("Sort is invalid");
        }



        private bool ValidateFilterString(string input)
        {
            bool isValid = input.Contains(',') ? !input.Split(',').ToList().Any(e => validateFilterFormat(e) == false) : validateFilterFormat(input);
            return isValid;
        }

        private bool validateFilterFormat(string input)
        {
            return string.IsNullOrEmpty(input) ? false :
                        input.Contains(':') &&
                        input.Split(':').Length == 3 &&
                        filterOperators.Any(e => e == input.Split(':')[1].ToLower());
        }

        private bool ValidateSortString(string input)
        {
            bool isValid = input.Contains(',') ? !input.Split(',').ToList().Any(e => ValidateSortFormat(e) == false)
            : ValidateSortFormat(input);
            return isValid;
        }

        private bool ValidateSortFormat(string input)
        {
            return string.IsNullOrEmpty(input) ? false :
                        input.Contains(':') &&
                        input.Split(':').Length == 2 &&
                        sortDirection.Any(e => e == input.Split(':')[1].ToLower());
        }
    }
}