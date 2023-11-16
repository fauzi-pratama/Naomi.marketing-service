using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("promotion_approval_detail")]
    public class PromotionApprovalDetail
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [ForeignKey("promotion_approval")]
        [Column("promotion_approval_id", Order = 1)]
        public Guid? PromotionApprovalId { get; set; }

        [Required]
        [Column("approval_level", Order = 2)]
        public int ApprovalLevel { get; set; }

        [Required]
        [Column("approver_id", Order = 3), MaxLength(50)]
        public string? ApproverId { get; set; }  //ini ada MasterTable-nya ga? kalo ada nanti diubah jadi FK

        [Column("job_position",Order = 4), MaxLength(200)]
        public string? JobPosition { get; set; }

        [Column("approve", Order = 5)]
        public bool Approve { get; set; } = false;

        [Column("approval_date", Order = 6)]
        public DateTime? ApprovalDate { get; set; }

        [Column("approval_notes", Order = 7)]
        public string? ApprovalNotes { get; set; }


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
