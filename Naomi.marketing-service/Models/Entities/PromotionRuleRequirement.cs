using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("promotion_rule_requirement")]
    public class PromotionRuleRequirement
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [ForeignKey("promotion_header")]
        [Column("promotion_header_id", Order = 1)]
        public Guid PromotionHeaderId { get; set; }

        [Column("line_num", Order = 2)]
        public int LineNum { get; set; }

        [Column("stock_code_id", Order = 3)]
        public Guid StockCodeId { get; set; }

        [Column("stock_code", Order = 4), MaxLength(50)]
        public string? StockCode { get; set; }

        [Column("stock_name", Order = 5), MaxLength(200)]
        public string? StockName { get; set; }

        [Column("qty", Order = 6)]
        public int Qty { get; set; }

        [Column("price", Order = 7)]
        public string? Price { get; set; }


        [Column("created_date", Order = 8)]
        public DateTime CreatedDate { get; set; }

        [Column("created_by", Order = 9), MaxLength(50)]
        public string? CreatedBy { get; set; }

        [Column("updated_date", Order = 10)]
        public DateTime UpdatedDate { get; set; }

        [Column("updated_by", Order = 11), MaxLength(50)]
        public string? UpdatedBy { get; set; }

        [Required]
        [Column("active_flag", Order = 12)]
        public bool ActiveFlag { get; set; }
    }
}
