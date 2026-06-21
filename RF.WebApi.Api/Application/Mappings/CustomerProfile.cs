using AutoMapper;
using RF.WebApi.Api.Application.DTOs.Customer;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AccountId, opt => opt.Ignore())
                .ForMember(dest => dest.SellingBills, opt => opt.Ignore());
            CreateMap<UpdateCustomerDto, Customer>()
                .ForMember(dest => dest.AccountId, opt => opt.Ignore())
                .ForMember(dest => dest.SellingBills, opt => opt.Ignore());
            CreateMap<Customer, CustomerListDto>()
                .ForMember(dest => dest.TotalPurchases, opt => opt.Ignore())
                .ForMember(dest => dest.TotalNetAmount, opt => opt.Ignore())
                .ForMember(dest => dest.TotalRemainingAmount, opt => opt.Ignore());
        }
    }
}
