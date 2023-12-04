namespace Naomi.marketing_service.Models.Response
{
    public class ApprovalMappingResponse
    {
        public class ApprovalMappingView
        {
            public Guid? Id { get; set; }
            public Guid? CompanyId { get; set; }
            public string? CompanyCode { get; set; }
            public List<ApprovalMappingViewDetail>? ApprovalMappingList { get; set; }
            public bool ActiveFlag { get; set; }
            public string? Username { get; set; }
        }
        public class ApprovalMappingViewDetail
        {
            public Guid? Id { get; set; }
            public int ApprovalLevel { get; set; }

            public string? ApproverId { get; set; }

            public string? JobPosition { get; set; }

        }
    }
}
