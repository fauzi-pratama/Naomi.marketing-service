using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("mop_view_model")]
    public class MopViewModel
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [Required]
        [Column("company_code", Order = 1), MaxLength(50)]
        public string? CompanyCode { get; set; }

        [Column("site_code", Order = 2), MaxLength(50)]
        public string? SiteCode { get; set; }

        [Required]
        [Column("mop_code", Order = 3), MaxLength(50)]
        public string? MopCode { get; set; }

        [Required]
        [Column("mop_name", Order = 4), MaxLength(200)]
        public string? MopName { get; set; }

        [Column("is_promotion", Order = 5)]
        public bool IsPromotion { get; set; } = false;


        [Column("created_date", Order = 6)]
        public DateTime CreatedDate { get; set; }

        [Column("created_by", Order = 7), MaxLength(50)]
        public string? CreatedBy { get; set; }

        [Column("updated_date", Order = 8)]
        public DateTime UpdatedDate { get; set; }

        [Column("updated_by", Order = 9), MaxLength(50)]
        public string? UpdatedBy { get; set; }

        [Required]
        [Column("active_flag", Order = 10)]
        public bool ActiveFlag { get; set; }
    }
}
