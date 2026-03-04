using AutoMapper;
using RF.WebApi.Api.Application.DTOs.BusinessExpence;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class BusinessExpenceProfile : Profile
    {
        public BusinessExpenceProfile()
        {
            CreateMap<BusinessExpence, BusinessExpenceDto>();
            CreateMap<CreateBusinessExpenceDto, BusinessExpence>();
            CreateMap<UpdateBusinessExpenceDto, BusinessExpence>();

            CreateMap<BusinessExpencePayment, BusinessExpencePaymentDto>();
            CreateMap<CreateBusinessExpencePaymentDto, BusinessExpencePayment>();
            CreateMap<UpdateBusinessExpencePaymentDto, BusinessExpencePayment>();
        }
    }
}
