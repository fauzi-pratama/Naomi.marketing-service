using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("promotion_history")]
    public class PromotionHistory
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [ForeignKey("promotion_header")]
        [Column("promotion_header_id", Order = 1)]
        public Guid PromotionHeaderId { get; set; }

        [ForeignKey("promotion_status")]
        [Column("promotion_status_id", Order = 2)]
        public Guid PromotionStatusId { get; set; }


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
