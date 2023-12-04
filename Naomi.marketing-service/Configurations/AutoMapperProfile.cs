
using AutoMapper;
using Naomi.marketing_service.Models.Entities;
using static Naomi.marketing_service.Models.Request.PromotionClassRequest;
using static Naomi.marketing_service.Models.Request.PromotionTypeRequest;
using static Naomi.marketing_service.Models.Request.ChannelMaterialRequest;
using static Naomi.marketing_service.Models.Request.PromotionStatusRequest;
using Naomi.marketing_service.Models.Request;
using static Naomi.marketing_service.Models.Response.ApprovalMappingResponse;

namespace Naomi.marketing_service.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreatePromotionClass, PromotionClass>();
            CreateMap<UpdatePromotionClass, PromotionClass>();
            
            CreateMap<CreatePromotionType, PromotionType>();
            CreateMap<UpdatePromotionType, PromotionType>();

            CreateMap<PromoChannelRequest, PromotionChannel>();
            CreateMap<PromoMaterialRequest, PromotionMaterial>();
            CreateMap<CreatePromotionStatus, PromotionStatus>();
            CreateMap<AppDisplayRequest, PromotionAppDisplay>();
            CreateMap<AppDisplayEditRequest, PromotionAppDisplay>();

            CreateMap<CreateApprovalMapping, ApprovalMappingView>();
            CreateMap<UpdateApprovalMapping, ApprovalMappingView>();
            CreateMap<ApprovalMappingRequest, ApprovalMappingViewDetail>();
            CreateMap<ApprovalMappingDetail, ApprovalMappingViewDetail>();
        }
    }
}
