namespace Naomi.marketing_service.Models.Request
{
    public class PromotionDetailRequest
    {
        public class CreatePromoRuleReq
        {
            public int LineNum { get; set; }

            public Guid StockCodeId { get; set; }

            public string? StockCode { get; set; }
            public string? StockName { get; set; }

            public int Qty { get; set; }

            public string? Price { get; set; }
        }
        public class CreatePromoRuleResult
        {
            public int LineNum { get; set; }

            public Guid StockCodeId { get; set; }
            public string? StockCode { get; set; }
            public string? StockName { get; set; }

            public int Qty { get; set; }

            public string? Value { get; set; }

            public string? MaxDisc { get; set; }
        }
    }
}
