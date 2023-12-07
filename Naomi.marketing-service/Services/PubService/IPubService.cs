using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Message.Pub;

namespace Naomi.marketing_service.Services.PubService
{
    public interface IPubService
    {
        void SendPromoClassMessage(PromotionClass promotionClass, string CreateUpdate);
        void SendPromoTypeMessage(PromotionType promotionType, string CreateUpdate);
        void SendPromoCreatedMessage(PromoCreated promoCreated, string CreateUpdate);
    }
}
