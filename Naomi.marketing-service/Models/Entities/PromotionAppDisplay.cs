using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("promotion_app_display")]
    public class PromotionAppDisplay
    {

        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [Required]
        [Column("app_code", Order = 1), MaxLength(50)]
        public string? AppCode { get; set; }

        [Required]
        [Column("app_name", Order = 2), MaxLength(200)]
        public string? AppName { get; set; }

        [Column("bucket_name", Order = 3), MaxLength(200)]
        public string? BucketName { get; set; }

        [Column("region", Order = 4), MaxLength(200)]
        public string? Region { get; set; }

        [Column("secret_key", Order = 5), MaxLength(200)]
        public string? SecretKey { get; set; }

        [Column("access_key", Order = 6), MaxLength(200)]
        public string? AccessKey { get; set; }

        [Column("base_directory", Order = 7), MaxLength(200)]
        public string? BaseDirectory { get; set; }


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


        [NotMapped]
        public string? Username { get; set; }
    }
}
