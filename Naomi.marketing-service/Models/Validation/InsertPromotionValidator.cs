using FluentValidation;
using Naomi.marketing_service.Models.Request;

namespace Naomi.marketing_service.Models.Validation
{
    public class InsertPromotionValidator : AbstractValidator<CreatePromotion>
    {
        public InsertPromotionValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.PromotionName).NotNull().WithMessage("Promotion Name is required")
                                         .NotEmpty().WithMessage("Promotion Name is required")
                                         .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Promotion Name is required")
                                         .NotEqual("null", StringComparer.OrdinalIgnoreCase).WithMessage("Promotion Name is required")
                                         .MaximumLength(200).WithMessage("Promotion name must be 200 chars or less");

            RuleFor(x => x.RedemptionCode).NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Please check Redemption Code");

            #region Company
            RuleFor(x => x.CompanyId).NotEqual(Guid.Empty).WithMessage("Company is required");

            RuleFor(x => x.CompanyCode).NotNull().WithMessage("Company is required")
                                       .NotEmpty().WithMessage("Company is required")
                                       .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Company is required")
                                       .NotEqual("mull", StringComparer.OrdinalIgnoreCase).WithMessage("Company is required")
                                       .MaximumLength(50).WithMessage("Status name must be 50 chars or less");
            
            RuleFor(x => x.CompanyName).NotNull().WithMessage("Company is required")
                                       .NotEmpty().WithMessage("Company is required")
                                       .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Company is required")
                                       .NotEqual("mull", StringComparer.OrdinalIgnoreCase).WithMessage("Company is required")
                                       .MaximumLength(200).WithMessage("Company name must be 200 chars or less");
            #endregion

            RuleFor(x => x.Depts).NotEmpty().WithMessage("Depts is required")
                                 .NotNull().WithMessage("Depts is required")
                                 .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Depts is required")
                                 .NotEqual("null", StringComparer.OrdinalIgnoreCase).WithMessage("Depts is required")
                                 .NotEqual("[]").WithMessage("Depts is required");

            RuleFor(x => x.PromotionChannel).NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Please check Promotion Channels");

            RuleFor(x => x.Objective).NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Please check Objective");

            RuleFor(x => x.PromotionMaterial).NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Please check Promotion Material");

            RuleFor(x => x.Zones).NotEmpty().WithMessage("Zones is required")
                                 .NotNull().WithMessage("Zones is required")
                                 .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Zones is required")
                                 .NotEqual("null", StringComparer.OrdinalIgnoreCase).WithMessage("Zones is required")
                                 .NotEqual("[]").WithMessage("Zones is required");

            RuleFor(x => x.Sites).NotEmpty().WithMessage("Sites is required")
                                 .NotNull().WithMessage("Sites is required")
                                 .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Sites is required")
                                 .NotEqual("null", StringComparer.OrdinalIgnoreCase).WithMessage("Sites is required")
                                 .NotEqual("[]").WithMessage("Sites is required");

            RuleFor(x => x.MaxPromoUsedType).NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Please check Soft Booking");

            RuleFor(x => x.MaxPromoUsedQty).NotEqual(0)
                                           .When(x => !string.IsNullOrEmpty(x.MaxPromoUsedType))
                                           .WithMessage("Please check Soft Booking Qty");

            RuleFor(x => x.PromotionClassId).NotEqual(Guid.Empty).WithMessage("Promotion Class is required");

            RuleFor(x => x.Value).NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Please check Value");

            RuleFor(x => x.MaxDisc).NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Please check Max Disc");

            RuleFor(x => x.PromoDisplayed).NotNull().WithMessage("Please insert Promo Displayed")
                                          .NotEmpty().WithMessage("Please insert Promo Displayed")
                                          .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Please insert Promo Displayed")
                                          .NotEqual("null", StringComparer.OrdinalIgnoreCase).WithMessage("Please insert Promo Displayed")
                                          .NotEqual("[]", StringComparer.OrdinalIgnoreCase).WithMessage("Please insert Promo Displayed");

            RuleFor(x => x.ShortDesc).NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Please check Short Description");

            RuleFor(x => x.PromoTermsCondition).NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Please check Terms & Condition");
        }
    }
}
