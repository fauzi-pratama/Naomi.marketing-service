using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("promotion_channel")]
    public class PromotionChannel
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [Required]
        [Column("channel_name", Order = 1), MaxLength(200)]
        public string? ChannelName { get; set; }


        [Column("created_date", Order = 2)]
        public DateTime CreatedDate { get; set; }

        [Column("created_by", Order = 3), MaxLength(50)]
        public string? CreatedBy { get; set; }

        [Column("updated_date", Order = 4)]
        public DateTime UpdatedDate { get; set; }

        [Column("updated_by", Order = 5), MaxLength(50)]
        public string? UpdatedBy { get; set; }

        [Required]
        [Column("active_flag", Order = 6)]
        public bool ActiveFlag { get; set; }


        [NotMapped]
        public string? Username { get; set; }
    }
}
