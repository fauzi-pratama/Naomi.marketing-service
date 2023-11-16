using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("promotion_entertain")]
    public class PromotionEntertain
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [Required]
        [Column("employee_nip", Order = 1), MaxLength(50)]
        public string? EmployeeNIP { get; set; }

        [Column("employee_name", Order = 2), MaxLength(200)]
        public string? EmployeeName { get; set; }

        [Required]
        [Column("job_position", Order = 3), MaxLength(200)]
        public string? JobPosition { get; set; }

        [Column("start_date", Order = 4)]
        public DateTime? StartDate { set; get; }

        [Column("end_date", Order = 5)]
        public DateTime? EndDate { set; get; }

        [Required]
        [Column("entertain_budget", Order = 6)]
        public decimal? EntertainBudget { get; set; }


        public List<PromotionEntertainEmail>? EmpEmails { get; set; }


        [Column("created_date", Order = 7)]
        public DateTime CreatedDate { get; set; }

        [Column("created_by", Order = 8), MaxLength(50)]
        public string? CreatedBy { get; set; }

        [Column("updated_date", Order = 9)]
        public DateTime UpdatedDate { get; set; }

        [Column("updated_by", Order = 10), MaxLength(50)]
        public string? UpdatedBy { get; set; }

        [Required]
        [Column("active_flag", Order = 11)]
        public bool ActiveFlag { get; set; }
    }
}
