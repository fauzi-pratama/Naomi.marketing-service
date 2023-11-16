using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("promotion_class")]
    public class PromotionClass
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [Required]
        [Column("promotion_class_key", Order = 1), MaxLength(50)]
        public string? PromotionClassKey { get; set; }

        [Required]
        [Column("promotion_class_name", Order = 2), MaxLength(200)]
        public string? PromotionClassName { get; set; }

        //ITEM = 1 ELSE = 2
        [Column("line_num", Order = 3)]
        public int LineNum { get; set; }


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
