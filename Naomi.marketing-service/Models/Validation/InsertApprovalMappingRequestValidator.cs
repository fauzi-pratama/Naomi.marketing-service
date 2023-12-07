using FluentValidation;
using Naomi.marketing_service.Models.Request;

namespace Naomi.marketing_service.Models.Validation
{
    public class InsertApprovalMappingRequestValidator : AbstractValidator<CreateApprovalMapping>
    {
        public InsertApprovalMappingRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            #region CompanyId
            RuleFor(x => x.CompanyId).NotNull()
                                     .NotEmpty()
                                     .NotEqual(Guid.Empty)
                                     .When(x => string.IsNullOrEmpty(x.CompanyCode))
                                     .WithMessage("Company is required");
            #endregion

            #region CompanyCode
            RuleFor(x => x.CompanyCode).NotNull()
                                       .NotEmpty()
                                       .When(x => x.CompanyId == Guid.Empty)
                                       .WithMessage("Company is required");
            #endregion

            #region List of approver
            RuleFor(x => x.ApprovalMappingList).NotNull()
                                               .NotEmpty()
                                               .WithMessage("Approver is required");
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
