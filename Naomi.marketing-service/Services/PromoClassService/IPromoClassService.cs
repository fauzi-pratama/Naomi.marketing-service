using Naomi.marketing_service.Models.Entities;
using static Naomi.marketing_service.Models.Request.PromotionClassRequest;

namespace Naomi.marketing_service.Services.PromoClassService
{
    public interface IPromoClassService
    {
        Task<List<PromotionClass>> GetPromotionClass(Guid id);
        Task<Tuple<PromotionClass, string>> GetPromotionClassByIdAsync(Guid id);
        Task<Tuple<PromotionClass, string>> InsertPromotionClass(PromotionClass newPromoClass);
        Task<Tuple<PromotionClass, string>> UpdatePromotionClass(PromotionClass updatedPromoClass);
    }
}
