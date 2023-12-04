using Naomi.marketing_service.Models.Entities;

namespace Naomi.marketing_service.Services.PubService
{
    public interface IPubService
    {
        void SendPromoClassMessage(PromotionClass promotionClass, string CreateUpdate);
        void SendPromoTypeMessage(PromotionType promotionType, string CreateUpdate);
        void SendPromoCreatedMessage(PromotionHeader promoHeader, string CreateUpdate);
    }
}
