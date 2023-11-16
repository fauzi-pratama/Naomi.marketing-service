using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("company_view_model")]
    public class CompanyViewModel
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [Required]
        [Column("company_code", Order = 1), MaxLength(50)]
        public string? CompanyCode { get; set; }

        [Required]
        [Column("company_description", Order = 2), MaxLength(200)]
        public string? CompanyDescription { get; set; }


        [Column("created_date", Order = 3)]
        public DateTime CreatedDate { get; set; }

        [Column("created_by", Order = 4), MaxLength(50)]
        public string? CreatedBy { get; set; }

        [Column("updated_date", Order = 5)]
        public DateTime UpdatedDate { get; set; }

        [Column("updated_by", Order = 6), MaxLength(50)]
        public string? UpdatedBy { get; set; }

        [Required]
        [Column("active_flag", Order = 7)]
        public bool ActiveFlag { get; set; }
    }
}
