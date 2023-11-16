using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("site_view_model")]
    public class SiteViewModel
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [Column("company_code", Order = 1), MaxLength(50)]
        public string? CompanyCode { get; set; }

        [Column("site_code", Order = 2), MaxLength(50)]
        public string? SiteCode { get; set; }

        [Column("site_description", Order = 3), MaxLength(200)]
        public string? SiteDescription { get; set; }


        [Column("created_date", Order = 4)]
        public DateTime CreatedDate { get; set; }

        [Column("created_by", Order = 5), MaxLength(50)]
        public string? CreatedBy { get; set; }

        [Column("updated_date", Order = 6)]
        public DateTime UpdatedDate { get; set; }

        [Column("updated_by", Order = 7), MaxLength(50)]
        public string? UpdatedBy { get; set; }

        [Required]
        [Column("active_flag", Order = 8)]
        public bool ActiveFlag { get; set; }
    }
}
