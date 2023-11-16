﻿namespace Naomi.marketing_service.Models.Message
{
    public class PromotionTypeUpdated
    {
        public Guid Id { get; set; }
        public Guid PromotionClassId { get; set; }
        public string? PromotionTypeKey { get; set; }
        public string? PromotionTypeName { get; set; }
        public int LineNum { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string? LastUpdatedBy { get; set; }
        public bool ActiveFlag { get; set; }
    }
}
