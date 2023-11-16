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


        [Column("created_date", Order = 5)]
        public DateTime CreatedDate { get; set; }

        [Column("created_by", Order = 6), MaxLength(50)]
        public string? CreatedBy { get; set; }

        [Column("updated_date", Order = 7)]
        public DateTime UpdatedDate { get; set; }

        [Column("updated_by", Order = 8), MaxLength(50)]
        public string? UpdatedBy { get; set; }

        [Required]
        [Column("active_flag", Order = 9)]
        public bool ActiveFlag { get; set; }
    }
}
