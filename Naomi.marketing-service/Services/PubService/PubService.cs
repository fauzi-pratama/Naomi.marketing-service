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

            var topicName = CreateUpdate == "Create" ? "promo_class_created" : "promo_class_updated";
            _logger.LogInformation("attempt to publish topic {topicName}", topicName);
            try
            {
                _capPublisher.Publish(topicName, data);
                _logger.LogInformation("Topic {topicName} published successfully", topicName);
            }
            catch (Exception ex)
            {
                _logger.LogError("error publish topic {topicName}. {ex.Message}", topicName, ex.Message);
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

            var topicName = CreateUpdate == "Create" ? "promo_type_created" : "promo_type_updated";
            _logger.LogInformation("attempt to publish topic {topicName}", topicName);
            try
            {
                _capPublisher.Publish(topicName, data);
                _logger.LogInformation("Topic {topicName} published successfully", topicName);
            }
            catch (Exception ex)
            {
                _logger.LogError("error publish topic {topicName}. {ex.Message}", topicName, ex.Message);
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

            var topicName = CreateUpdate == "Create" ? "promo_created" : "promo_updated";
            _logger.LogInformation("attempt to publish topic {topicName}", topicName);
            try
            {
                _capPublisher.Publish(topicName, data);
                _logger.LogInformation("Topic {topicName} published successfully", topicName);
            }
            catch (Exception ex)
            {
                _logger.LogError("error publish topic {topicName}. {ex.Message}", topicName, ex.Message);
            }
        }

        public void SendPromoEmailUserMessage(PromoEmailUser promoEmailUser)
        {
            Dictionary<string, object> dataDetail = new();
            foreach (PropertyInfo pi in promoEmailUser.GetType().GetProperties())
            {
                dataDetail.Add(pi.Name, pi.GetValue(promoEmailUser)!);
            }

            Dictionary<string, object> data = new()
            {
                {"SyncName", "promo_email_user" },
                {"DocumentNumber", promoEmailUser.Nip! },
                {"SyncData", dataDetail}
            };

            var topicName = "promo_email_user";
            _logger.LogInformation("attempt to publish topic {topicName}", topicName);
            try
            {
                _capPublisher.Publish(topicName, data);
                _logger.LogInformation("Topic {topicName} published successfully", topicName);
            }
            catch (Exception ex)
            {
                _logger.LogError("error publish topic {topicName}. {ex.Message}", topicName, ex.Message);
            }
        }
    }
}
