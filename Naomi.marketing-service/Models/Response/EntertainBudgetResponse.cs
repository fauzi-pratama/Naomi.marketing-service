namespace Naomi.marketing_service.Models.Response
{
    public class EntertainBudgetResponse
    {
        public class PromoEntertainListView
        {
            public Guid Id { get; set; }
            public string? EmployeeNIP { get; set; }
            public string? EmployeeName { get; set; }
            public string? JobPosition { get; set; }
            public DateTime? StartDate { set; get; } = DateTime.Parse("1/1/1900");
            public DateTime? EndDate { set; get; } = DateTime.Parse("1/1/1900");
            public decimal? EntertainBudget { get; set; }
        }
    }
}
