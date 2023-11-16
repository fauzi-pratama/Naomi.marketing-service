using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("approval_mapping_detail")]
    public class ApprovalMappingDetail
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [ForeignKey("approval_mapping")]
        [Column("approval_mapping_id", Order = 1)]
        public Guid ApprovalMappingId { get; set; }

        [Required]
        [Column("approval_level", Order = 2)]
        public int ApprovalLevel { get; set; }

        [Required]
        [Column("approver_id", Order = 3), MaxLength(50)]
        public string? ApproverId { get; set; }  

        [Column("job_position", Order = 4),MaxLength(50)]
        public string? JobPosition { get; set; }


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
