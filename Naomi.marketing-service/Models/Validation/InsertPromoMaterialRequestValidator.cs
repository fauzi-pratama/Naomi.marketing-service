using FluentValidation;
using static Naomi.marketing_service.Models.Request.ChannelMaterialRequest;

namespace Naomi.marketing_service.Models.Validation
{
    public class InsertPromoMaterialRequestValidator : AbstractValidator<PromoMaterialRequest>
    {
        public InsertPromoMaterialRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            #region MaterialName
            RuleFor(x => x.MaterialName).NotNull().WithMessage("Material name is required")
                                        .NotEmpty().WithMessage("Material name is required")
                                        .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Material name is required")
                                        .MaximumLength(200).WithMessage("Material name must be 200 chars or less");
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
