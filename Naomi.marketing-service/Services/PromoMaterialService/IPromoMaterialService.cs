using Naomi.marketing_service.Models.Entities;
using static Naomi.marketing_service.Models.Request.ChannelMaterialRequest;

namespace Naomi.marketing_service.Services.PromoMaterialService
{
    public interface IPromoMaterialService
    {
        Task<Tuple<List<PromotionMaterial>, int>> GetPromotionMaterial(string? searchName, int pageNo, int pageSize);
        Task<Tuple<PromotionMaterial, string>> InsertPromotionMaterial(PromotionMaterial newPromoMaterial);
    }
}
