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
            CreateMap<UpdateBusinessExpenceDto, BusinessExpence>()
                .ForMember(dest => dest.Payments, opt => opt.Ignore());

            CreateMap<BusinessExpence, BusinessExpenceListDto>()
                .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => src.Payments.Sum(p => p.Amount ?? 0)));

            CreateMap<BusinessExpencePayment, BusinessExpencePaymentDto>();
            CreateMap<CreateBusinessExpencePaymentDto, BusinessExpencePayment>();
            CreateMap<UpdateBusinessExpencePaymentDto, BusinessExpencePayment>();
        }
    }
}
