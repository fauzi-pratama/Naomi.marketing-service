using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("promotion_app_image")]
    public class PromotionAppImage
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [ForeignKey("promotion_header")]
        [Column("promotion_header_id", Order = 1)]
        public Guid PromotionHeaderId { get; set; }

        [Required]
        [Column("app_code", Order = 2), MaxLength(200)]
        public string? AppCode { get; set; }

        [Column("image_link", Order = 3), DataType("text")]
        public string? ImageLink { get; set; }

        [Column("file_name", Order = 4), MaxLength(50)]
        public string? FileName { get; set; }


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
