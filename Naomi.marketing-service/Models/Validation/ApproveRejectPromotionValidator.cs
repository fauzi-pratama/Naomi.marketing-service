using FluentValidation;
using Naomi.marketing_service.Models.Request;

namespace Naomi.marketing_service.Models.Validation
{
    public class ApproveRejectPromotionValidator : AbstractValidator<ApproveRejectPromotion>
    {
        public ApproveRejectPromotionValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            #region PromotioHeaderId
            RuleFor(x => x.PromotionHeaderId).NotNull()
                                             .NotEmpty()
                                             .NotEqual(Guid.Empty)
                                             .WithMessage("Promotion Id is required");
            #endregion

            #region ApproverId
            RuleFor(x => x.ApproverId).NotNull()
                                      .NotEmpty()
                                      .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                      .WithMessage("Approver id is required");
            #endregion

            #region ApprovalNote
            RuleFor(x => x.ApprovalNotes).NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                         .WithMessage("Please check approval notes");
            #endregion
        }
    }
}
