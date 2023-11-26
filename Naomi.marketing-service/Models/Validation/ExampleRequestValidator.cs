
using FluentValidation;
using Naomi.marketing_service.Models.Request;

namespace Naomi.marketing_service.Models.Validation
{
    public class ExampleRequestValidator : AbstractValidator<ExampleRequest>
    {
        public ExampleRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        }
    }
}
