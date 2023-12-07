using FluentValidation;
using Naomi.marketing_service.Models.Request;

namespace Naomi.marketing_service.Models.Validation
{
    public class UpdateApprovalMappingValidator : AbstractValidator<UpdateApprovalMapping>
    {
        public UpdateApprovalMappingValidator() 
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            #region Id
            RuleFor(x => x.Id).NotNull()
                              .NotEmpty()
                              .NotEqual(Guid.Empty)
                              .WithMessage("Approval Mapping Id is required");
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
