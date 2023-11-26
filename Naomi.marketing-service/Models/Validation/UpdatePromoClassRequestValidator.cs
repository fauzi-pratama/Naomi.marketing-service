using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Request;
using static Naomi.marketing_service.Models.Request.PromotionClassRequest;

namespace Naomi.marketing_service.Models.Validation
{
    public class UpdatePromoClassRequestValidator : AbstractValidator<UpdatePromotionClass>
    {
        public UpdatePromoClassRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            #region Id
            RuleFor(x => x.Id).NotNull()
                              .NotEmpty()
                              .WithMessage("Class id is required");
            #endregion

            #region Class Key
            RuleFor(x => x.PromotionClassKey).NotNull().WithMessage("Class key is required")
                                             .NotEmpty().WithMessage("Class key is required")
                                             .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Class key is required")
                                             .MaximumLength(50).WithMessage("Class key must be 50 chars or less");
            #endregion

            #region Class Name
            RuleFor(x => x.PromotionClassName).NotNull().WithMessage("Class name is required")
                                              .NotEmpty().WithMessage("Class name is required")
                                              .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Class name is required")
                                              .MaximumLength(200).WithMessage("Class name must be 200 chars or less");
            #endregion

            #region Username
            RuleFor(x => x.Username).NotNull().WithMessage("Username is required")
                                    .NotEmpty().WithMessage("Username is required")
                                    .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Username is required")
                                    .MaximumLength(50).WithMessage("Username must be 50 chars or less");
            #endregion
        }
    }
}
