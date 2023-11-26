using Naomi.marketing_service.Models.Entities;
using static Naomi.marketing_service.Models.Request.PromotionTypeRequest;

namespace Naomi.marketing_service.Services.PromoTypeService
{
    public interface IPromoTypeService
    {
        Task<List<PromotionType>> GetPromotionType(Guid id);
        Task<Tuple<PromotionType, string>> GetPromotionTypeByIdAsync(Guid? id);
        Task<Tuple<List<PromotionType>, string>> GetPromotionTypeByClassIdAsync(Guid classId);
        Task<Tuple<PromotionType, string>> InsertPromotionType(PromotionType newPromoType);
        Task<Tuple<PromotionType, string>> UpdatePromotionType(PromotionType updatedPromoType);
    }
}
