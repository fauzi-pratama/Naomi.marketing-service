using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("promotion_status")]
    public class PromotionStatus
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [Column("promotion_status_key", Order = 1), MaxLength(50)]
        public string? PromotionStatusKey { get; set; }

        [Column("promotion_status_name", Order = 2), MaxLength(200)]
        public string? PromotionStatusName { get; set; }


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
