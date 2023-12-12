namespace Naomi.marketing_service.Models.Request
{
    public class EntertainBudgetRequest
    {
        public class CreateEntertain
        {
            public string? EmployeeNIP { get; set; }
            public string? EmployeeName { get; set; }
            public string? JobPosition { get; set; }
            public decimal? EntertainBudget { get; set; }
            public DateTime MonthYear { get; set; } = DateTime.UtcNow.Date;
            public bool? ActiveFlag { get; set; }
            public List<EmpEmail>? EmpEmails { get; set; }
            public string? Username { get; set; }
        }

        public class UpdateEntertain
        {
            public Guid Id { get; set; }
            public decimal? EntertainBudget { get; set; }
            public DateTime MonthYear { get; set; } = DateTime.UtcNow.Date;
            public bool? ActiveFlag { get; set; }
            public List<EmpEmail>? EmpEmails { get; set; }
            public string? Username { get; set; }
        }

        public class EmpEmail
        {
            public string? Email { get; set; }
            public bool DefaultEmail { get; set; } = false;
        }
    }
}
