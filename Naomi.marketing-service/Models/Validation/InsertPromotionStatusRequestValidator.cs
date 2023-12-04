using FluentValidation;
using static Naomi.marketing_service.Models.Request.PromotionClassRequest;
using static Naomi.marketing_service.Models.Request.PromotionStatusRequest;

namespace Naomi.marketing_service.Models.Validation
{
    public class InsertPromotionStatusRequestValidator : AbstractValidator<CreatePromotionStatus>
    {
        public InsertPromotionStatusRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            #region StatusKey
            RuleFor(x => x.PromotionStatusKey).NotNull().WithMessage("Status key is required")
                                              .NotEmpty().WithMessage("Status key is required")
                                              .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Status key is required")
                                              .MaximumLength(50).WithMessage("Status key must be 50 chars or less");
            #endregion

            #region StatusName
            RuleFor(x => x.PromotionStatusName).NotNull().WithMessage("Status name is required")
                                               .NotEmpty().WithMessage("Status name is required")
                                               .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Status name is required")
                                               .MaximumLength(200).WithMessage("Status name must be 200 chars or less");
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
