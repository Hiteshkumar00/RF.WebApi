using AutoMapper;
using RF.WebApi.Api.Application.DTOs.Entity;
using RF.WebApi.Api.Application.DTOs.RelatedEntity;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<Entity, EntityDto>().ReverseMap();
            CreateMap<CreateEntityDto, Entity>();
            CreateMap<UpdateEntityDto, Entity>()
                .ForMember(dest => dest.RelatedEntities, opt => opt.Ignore());

            CreateMap<RelatedEntity, RelatedEntityDto>().ReverseMap();
            CreateMap<CreateRelatedEntityDto, RelatedEntity>();
            CreateMap<UpdateRelatedEntityDto, RelatedEntity>();
        }
    }
}
