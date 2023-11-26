using Naomi.marketing_service.Models.Entities;

namespace Naomi.marketing_service.Services.PromoAppService
{
    public interface IPromoAppService
    {
        Task<Tuple<List<PromotionAppDisplay>, int>> GetPromotionAppDisplay(string? searchName, int pageNo, int pageSize);
        Task<Tuple<PromotionAppDisplay, string>> InsertPromotionAppDisplay(PromotionAppDisplay newPromoAppDisplay);
        Task<Tuple<PromotionAppDisplay, string>> UpdatePromotionAppDisplay(PromotionAppDisplay promoAppDisplayEdit);
    }
}
