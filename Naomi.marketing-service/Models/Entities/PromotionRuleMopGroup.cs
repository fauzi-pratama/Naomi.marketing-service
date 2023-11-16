using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("promotion_rule_mop_group")]
    public class PromotionRuleMopGroup
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [ForeignKey("promotion_header")]
        [Column("promotion_header_id", Order = 1)]
        public Guid PromotionHeaderId { get; set; }

        [Column("line_num", Order = 2)]
        public int LineNum { get; set; }

        [Column("mop_group_id", Order = 3)]
        public string? MopGroupId { get; set; }

        [Column("mop_group_code", Order = 4), MaxLength(50)]
        public string? MopGroupCode { get; set; }

        [Column("mop_group_name", Order = 5), MaxLength(200)]
        public string? MopGroupName { get; set; }


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
