using DotNetCore.CAP;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using static Naomi.marketing_service.Models.Response.PromotionHeaderResponse;

namespace Naomi.marketing_service.Services.PromotionService
{
    public interface IPromotionService
    {
        Task<Tuple<List<PromotionListView>, int, string>> GetPromotionListAsync(string orderColumn, string orderMethod, int pageNo = 1, int pageSize = 10, PromotionsViewSearch? viewSearch = null);
        Task<PromotionViewModel> GetPromotionViewAsync(Guid promoHeaderId, string userId);
        Task<Tuple<List<PromotionListView>, int, string>> GetPromotionApprovalListAsync(string userId, string orderColumn, string orderMethod, int pageNo = 1, int pageSize = 10, PromotionsViewSearch? viewSearch = null);
        Task<Tuple<PromotionHeader, string>> CreatePromotion(CreatePromotion promotionHeader);
        Task<Tuple<PromotionHeader, string>> UpdatePromotion(UpdatePromotion promotionHeader);
        Task<PromotionHeader> UpdateActivePromo(Guid promoId, string? username, bool activeFlag);

        Task<Tuple<PromotionApprovalDetail, string>> ApproveRejectPromotion(ApproveRejectPromotion promoApproval);
        Task UpdatePromotionStatusByDate();

        #region EntertainJob
        Task CreatePromoEntertainAuto();
        Task PublishPromoEntertainAuto();
        #endregion
    }
}
