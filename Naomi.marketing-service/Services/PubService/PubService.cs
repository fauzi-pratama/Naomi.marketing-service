using Amazon.Runtime.Internal;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Message.Pub;
using System.Reflection;

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
            Dictionary<string, object> dataDetail = new();
            foreach (PropertyInfo pi in promotionClass.GetType().GetProperties())
            {
                dataDetail.Add(pi.Name, pi.GetValue(promotionClass)!);
            }

            Dictionary<string, object> data = new()
            {
                {"SyncName", CreateUpdate == "Create" ? "promo_class_created" : "promo_class_updated" },
                {"DocumentNumber", promotionClass.Id },
                {"SyncData", dataDetail}
            };

            _logger.LogInformation(string.Format("attempt to publish topic {0}", CreateUpdate == "Create" ? "promo_class_created" : "promo_class_updated"));
            try
            {
                _capPublisher.Publish(CreateUpdate == "Create" ? "promo_class_created" : "promo_class_updated", data);
                _logger.LogInformation(string.Format("Topic {0} published successfully", CreateUpdate == "Create" ? "promo_class_created" : "promo_class_updated"));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("error publish topic {0}. {1}", CreateUpdate == "Create" ? "promo_class_created" : "promo_class_updated", ex.Message));
            }
        }

        public void SendPromoTypeMessage(PromotionType promotionType, string CreateUpdate)
        {
            Dictionary<string, object> dataDetail = new();
            foreach (PropertyInfo pi in promotionType.GetType().GetProperties())
            {
                dataDetail.Add(pi.Name, pi.GetValue(promotionType)!);
            }

            Dictionary<string, object> data = new()
            {
                {"SyncName", CreateUpdate == "Create" ? "promo_type_created" : "promo_type_updated" },
                {"DocumentNumber", promotionType.Id },
                {"SyncData", dataDetail}
            };

            _logger.LogInformation(string.Format("attempt to publish topic {0}", CreateUpdate == "Create" ? "promo_type_created" : "promo_type_updated"));
            try
            {
                _capPublisher.Publish(CreateUpdate == "Create" ? "promo_type_created" : "promo_type_updated", data);
                _logger.LogInformation(string.Format("Topic {0} published successfully", CreateUpdate == "Create" ? "promo_type_created" : "promo_type_updated"));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("error publish topic {0}. {1}", CreateUpdate == "Create" ? "promo_type_created" : "promo_type_updated", ex.Message));
            }
        }

        public void SendPromoCreatedMessage(PromoCreated promoCreated, string CreateUpdate)
        {
            Dictionary<string, object> dataDetail = new();
            foreach (PropertyInfo pi in promoCreated.GetType().GetProperties())
            {
                dataDetail.Add(pi.Name, pi.GetValue(promoCreated)!);
            }

            Dictionary<string, object> data = new()
            {
                {"SyncName", CreateUpdate == "Create" ? "promo_created" : "promo_updated" },
                {"DocumentNumber", promoCreated.PromoRuleId! },
                {"SyncData", dataDetail}
            };

            _logger.LogInformation(string.Format("attempt to publish topic {0}", CreateUpdate == "Create" ? "promo_created" : "promo_updated"));
            try
            {
                _capPublisher.Publish(CreateUpdate == "Create" ? "promo_created" : "promo_updated", data);
                _logger.LogInformation(string.Format("Topic {0} published successfully", CreateUpdate == "Create" ? "promo_created" : "promo_updated"));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("error publish topic {0}. {1}", CreateUpdate == "Create" ? "promo_created" : "promo_updated", ex.Message));
            }
        }
    }
}
