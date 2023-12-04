using FluentValidation;
using static Naomi.marketing_service.Models.Request.ChannelMaterialRequest;
using static Naomi.marketing_service.Models.Request.PromotionClassRequest;

namespace Naomi.marketing_service.Models.Validation
{
    public class InsertPromoChannelRequestValidator : AbstractValidator<PromoChannelRequest>
    {
        public InsertPromoChannelRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            #region ChannelName
            RuleFor(x => x.ChannelName).NotNull().WithMessage("Channel name is required")
                                       .NotEmpty().WithMessage("Channel name is required")
                                       .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Channel name is required")
                                       .MaximumLength(200).WithMessage("Channel name must be 200 chars or less");
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
