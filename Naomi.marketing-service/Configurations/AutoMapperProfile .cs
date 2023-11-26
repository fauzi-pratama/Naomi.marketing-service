
using AutoMapper;
using Naomi.marketing_service.Models.Dto;
using Naomi.marketing_service.Models.Request;

namespace Naomi.marketing_service.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ExampleRequest, ExampleDto>()
                .ForMember(x => x.Id, c => c.MapFrom(q => q.Code));
        }
    }
}
