using FluentValidation;
using Naomi.marketing_service.Models.Request;

namespace Naomi.marketing_service.Models.Validation
{
    public class CreatePromotionValidator : AbstractValidator<CreatePromotion>
    {
        public CreatePromotionValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.PromotionName).NotNull().NotEmpty()
                                         .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                         .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                         .WithMessage("Promotion Name is required");

            RuleFor(x => x.RedemptionCode).NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                          .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                          .WithMessage("Please check Redemption Code");

            #region Company
            RuleFor(x => x.CompanyId).NotEqual(Guid.Empty)
                                     .WithMessage("Company is required");

            RuleFor(x => x.CompanyCode).NotNull().NotEmpty()
                                       .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                       .NotEqual("mull", StringComparer.OrdinalIgnoreCase)
                                       .WithMessage("Company is required");
            
            RuleFor(x => x.CompanyName).NotNull().NotEmpty()
                                       .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                       .NotEqual("mull", StringComparer.OrdinalIgnoreCase)
                                       .WithMessage("Company is required");
            #endregion

            RuleFor(x => x.Depts).NotEmpty().NotNull()
                                 .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                 .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                 .NotEqual("[]")
                                 .WithMessage("Depts is required");

            RuleFor(x => x.PromotionChannel).NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                            .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                            .WithMessage("Please check Promotion Channels");
            RuleFor(x => x.Objective).NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                     .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                     .WithMessage("Please check Objective");
            RuleFor(x => x.PromotionMaterial).NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                             .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                             .WithMessage("Please check Promotion Material");

            RuleFor(x => x.Zones).NotEmpty().NotNull()
                                 .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                 .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                 .NotEqual("[]")
                                 .WithMessage("Zones is required");
            RuleFor(x => x.Sites).NotEmpty().NotNull()
                                 .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                 .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                 .NotEqual("[]")
                                 .WithMessage("Sites is required");

            RuleFor(x => x.MaxPromoUsedType).NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                            .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                            .WithMessage("Please check Soft Booking");

            RuleFor(x => x.MaxPromoUsedQty).NotEqual(0)
                                           .When(x => !string.IsNullOrEmpty(x.MaxPromoUsedType))
                                           .WithMessage("Please check Soft Booking Qty");

            RuleFor(x => x.PromotionClassId).NotEqual(Guid.Empty)
                                            .WithMessage("Promotion Class is required");

            RuleFor(x => x.Value).NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                 .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                 .WithMessage("Please check Value");
            RuleFor(x => x.MaxDisc).NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                   .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                   .WithMessage("Please check Max Disc");

            RuleFor(x => x.PromoDisplayed).NotNull().NotEmpty()
                                          .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                          .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                          .NotEqual("[]", StringComparer.OrdinalIgnoreCase)
                                          .WithMessage("Please insert Promo Displayed");

            RuleFor(x => x.ShortDesc).NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                     .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                     .WithMessage("Please check Short Description");

            RuleFor(x => x.PromoTermsCondition).NotEqual("string", StringComparer.OrdinalIgnoreCase)
                                     .NotEqual("null", StringComparer.OrdinalIgnoreCase)
                                     .NotEqual("<p></p>", StringComparer.OrdinalIgnoreCase)
                                     .WithMessage("Please check Terms & Condition");
        }
    }
}
