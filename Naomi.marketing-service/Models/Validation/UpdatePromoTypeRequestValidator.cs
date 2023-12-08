﻿using FluentValidation;
using Naomi.marketing_service.Models.Request;

namespace Naomi.marketing_service.Models.Validation
{
    public class UpdatePromoTypeRequestValidator : AbstractValidator<PromotionTypeRequest.UpdatePromotionType>
    {
        public UpdatePromoTypeRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            #region Class Id
            RuleFor(x => x.PromotionClassId).NotNull().WithMessage("Promotion Class Id is required")
                                            .NotEmpty().WithMessage("Promotion Class Id is required")
                                            .NotEqual(Guid.Empty).WithMessage("Promotion Class Id is required");
            #endregion

            #region Type Id
            RuleFor(x => x.Id).NotNull().WithMessage("Type Id is required")
                              .NotEmpty().WithMessage("Type Id is required")
                              .NotEqual(Guid.Empty).WithMessage("Type Id is required");
            #endregion

            #region Type Key
            RuleFor(x => x.PromotionTypeKey).NotNull().WithMessage("Type key is required")
                                            .NotEmpty().WithMessage("Type key is required")
                                            .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Type key is required")
                                            .MaximumLength(50).WithMessage("Type key must be 50 chars or less");
            #endregion

            #region Type Name
            RuleFor(x => x.PromotionTypeName).NotNull().WithMessage("Type name is required")
                                             .NotEmpty().WithMessage("Type name is required")
                                             .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Type name is required")
                                             .MaximumLength(200).WithMessage("Type name must be 200 chars or less");
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
