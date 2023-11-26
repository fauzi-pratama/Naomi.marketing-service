using Naomi.marketing_service.Models.Entities;
using static Naomi.marketing_service.Models.Request.ChannelMaterialRequest;

namespace Naomi.marketing_service.Services.PromoChannelService
{
    public interface IPromoChannelService
    {
        Task<Tuple<List<PromotionChannel>, int>> GetPromotionChannel(string? searchName, int pageNo, int pageSize);
        Task<Tuple<PromotionChannel, string>> InsertPromotionChannel(PromotionChannel newPromoChannel);
    }
}
