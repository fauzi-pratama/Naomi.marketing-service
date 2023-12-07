
using AutoMapper;
using Naomi.marketing_service.Models.Entities;
using static Naomi.marketing_service.Models.Request.PromotionClassRequest;
using static Naomi.marketing_service.Models.Request.PromotionTypeRequest;
using static Naomi.marketing_service.Models.Request.ChannelMaterialRequest;
using static Naomi.marketing_service.Models.Request.PromotionStatusRequest;
using Naomi.marketing_service.Models.Request;
using static Naomi.marketing_service.Models.Response.ApprovalMappingResponse;
using Naomi.marketing_service.Models.Message.Pub;
using static Naomi.marketing_service.Models.Request.EntertainBudgetRequest;
using static Naomi.marketing_service.Models.Request.PromotionDetailRequest;

namespace Naomi.marketing_service.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreatePromotionClass, PromotionClass>();
            CreateMap<UpdatePromotionClass, PromotionClass>();
            CreateMap<PromotionClass, PromotionClassCreated>();
            CreateMap<PromotionClass, PromotionClassUpdated>();

            CreateMap<CreatePromotionType, PromotionType>();
            CreateMap<UpdatePromotionType, PromotionType>();
            CreateMap<PromotionType, PromotionTypeCreated>();
            CreateMap<PromotionType, PromotionTypeUpdated>();

            CreateMap<CreatePromotionStatus, PromotionStatus>();

            CreateMap<PromoChannelRequest, PromotionChannel>();
            CreateMap<PromoMaterialRequest, PromotionMaterial>();
            CreateMap<AppDisplayRequest, PromotionAppDisplay>();
            CreateMap<AppDisplayEditRequest, PromotionAppDisplay>();

            CreateMap<CreatePromotion, PromotionHeader>();
            CreateMap<PromotionHeader, CreatePromotion>();
            CreateMap<UpdatePromotion, PromotionHeader>();
            CreateMap<PromoCreated, PromoUpdated>();

            CreateMap<CreatePromoRuleReq, PromotionRuleRequirement>();
            CreateMap<PromotionRuleRequirement, CreatePromoRuleReq>();
            CreateMap<CreatePromoRuleResult, PromotionRuleResult>();
            CreateMap<PromotionRuleResult, CreatePromoRuleResult>();
            CreateMap<MopGroup, PromotionRuleMopGroup>();
            CreateMap<PromotionRuleMopGroup, MopGroup>();

            CreateMap<CreateApprovalMapping, ApprovalMappingView>();
            CreateMap<UpdateApprovalMapping, ApprovalMappingView>();
            CreateMap<ApprovalMappingRequest, ApprovalMappingViewDetail>();
            CreateMap<ApprovalMappingDetail, ApprovalMappingViewDetail>();

            CreateMap<PromotionEntertainEmail, EmpEmail>();
        }
    }
}
