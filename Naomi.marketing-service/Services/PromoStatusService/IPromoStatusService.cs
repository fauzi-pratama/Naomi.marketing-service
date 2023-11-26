using Naomi.marketing_service.Models.Entities;
using static Naomi.marketing_service.Models.Request.PromotionStatusRequest;
using static Naomi.marketing_service.Models.Response.PromotionStatusResponse;

namespace Naomi.marketing_service.Services.PromoStatusService
{
    public interface IPromoStatusService
    {
        Task<List<PromotionStatus>> GetPromotionStatus(Guid id);
        Task<PromotionStatus> GetPromotionStatusByIdAsync(Guid id);
        Task<PromotionStatus> GetPromotionStatusByNameAsync(string statusName);
        Task<List<RespondPromotionStatusCount>> GetPromotionStatusCount();
        Task<Tuple<PromotionStatus, string>> InsertPromotionStatus(PromotionStatus newPromoStatus);
    }
}
