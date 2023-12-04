using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;

namespace Naomi.marketing_service.Services.PubService
{
    public class PubService : IPubService
    {
        private readonly ICapPublisher _capPublisher;
        private readonly ILogger<PubService> _logger;

        public PubService(ICapPublisher capPublisher, ILogger<PubService> logger) 
        { 
            _capPublisher = capPublisher;
            _logger = logger;
        }

        public void SendPromoClassMessage(PromotionClass promotionClass, string CreateUpdate)
        {
            Dictionary<string, object> dataDetail = new()
            {
                { "Id", promotionClass.Id },
                { "LineNum", promotionClass.LineNum },
                { "PromotionClassKey", promotionClass.PromotionClassKey! },
                { "PromotionClassName", promotionClass.PromotionClassName! },
                { "ActiveFlag", promotionClass.ActiveFlag }
            };

            Dictionary<string, object> data = new()
            {
                {"SyncName", CreateUpdate == "Create" ? "promoclasscreated" : "promoclassupdated" },
                {"DocumentNumber", promotionClass.Id },
                {"SyncData", dataDetail}
            };

            _logger.LogInformation(string.Format("attempt to publish topic {0}", CreateUpdate == "Create" ? "promoclasscreated" : "promoclassupdated"));
            try
            {
                _capPublisher.Publish(CreateUpdate == "Create" ? "promoclasscreated" : "promoclassupdated", data);
                _logger.LogInformation(string.Format("Topic {0} published successfully", CreateUpdate == "Create" ? "promoclasscreated" : "promoclassupdated"));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("error publish topic {0}. {1}", CreateUpdate == "Create" ? "promoclasscreated" : "promoclassupdated", ex.Message));
            }
        }

        public void SendPromoTypeMessage(PromotionType promotionType, string CreateUpdate)
        {
            Dictionary<string, object> dataDetail = new()
            {
                { "PromotionClassId", promotionType.PromotionClassId },
                { "Id", promotionType.Id },
                { "LineNum", promotionType.LineNum },
                { "PromotionTypeKey", promotionType.PromotionTypeKey! },
                { "PromotionTypeName", promotionType.PromotionTypeName! },
                { "ActiveFlag", promotionType.ActiveFlag }
            };

            Dictionary<string, object> data = new()
            {
                {"SyncName", CreateUpdate == "Create" ? "promoclasscreated" : "promoclassupdated" },
                {"DocumentNumber", promotionType.Id },
                {"SyncData", dataDetail}
            };

            _logger.LogInformation(string.Format("attempt to publish topic {0}", CreateUpdate == "Create" ? "promotypecreated" : "promotypeupdated"));
            try
            {
                _capPublisher.Publish(CreateUpdate == "Create" ? "promotypecreated" : "promotypeupdated", data);
                _logger.LogInformation(string.Format("Topic {0} published successfully", CreateUpdate == "Create" ? "promotypecreated" : "promotypeupdated"));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("error publish topic {0}. {1}", CreateUpdate == "Create" ? "promotypecreated" : "promotypeupdated", ex.Message));
            }
        }

        public void SendPromoCreatedMessage(PromotionHeader promoHeader, string CreateUpdate)
        {
            //
        }
    }
}
