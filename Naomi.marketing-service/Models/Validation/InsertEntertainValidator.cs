using FluentValidation;
using Naomi.marketing_service.Models.Request;
using static Naomi.marketing_service.Models.Request.EntertainBudgetRequest;

namespace Naomi.marketing_service.Models.Validation
{
    public class InsertEntertainValidator : AbstractValidator<CreateEntertain>
    {
        public InsertEntertainValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.EmployeeNIP).NotNull().WithMessage("Employee NIP is required")
                                       .NotEmpty().WithMessage("Employee NIP is required")
                                       .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Employee NIP is required")
                                       .MaximumLength(50).WithMessage("Employee NIP must be 50 chars or less");

            RuleFor(x => x.EmployeeName).NotNull().WithMessage("Employee Name is required")
                                        .NotEmpty().WithMessage("Employee Name is required")
                                        .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Employee Name is required")
                                        .NotEqual("null", StringComparer.OrdinalIgnoreCase).WithMessage("Employee Name is required")
                                        .MaximumLength(200).WithMessage("Employee name must be 200 chars or less");

            RuleFor(x => x.JobPosition).NotNull().WithMessage("Job position is required")
                                       .NotEmpty().WithMessage("Job position is required")
                                       .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Job position is required")
                                       .NotEqual("null", StringComparer.OrdinalIgnoreCase).WithMessage("Job position is required")
                                       .MaximumLength(200).WithMessage("Job position must be 200 chars or less");

            RuleFor(x => x.EntertainBudget).NotNull().WithMessage("Budget is required")
                                           .NotEmpty().WithMessage("Budget is required")
                                           .NotEqual(0).WithMessage("Budget is required");
        }
    }
}
