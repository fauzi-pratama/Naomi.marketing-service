using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("promotion_entertain_email")]
    public class PromotionEntertainEmail
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [ForeignKey("promotion_entertain")]
        [Column("promotion_entertain_id", Order = 1)]
        public Guid? PromotionEntertainId { get; set; }

        [Required]
        [Column("email", Order = 2), MaxLength(50)]
        public string? Email { get; set; }

        [Required]
        [Column("default_email", Order = 3)]
        public bool? DefaultEmail { get; set; } = false;


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
