using AutoMapper;
using RF.WebApi.Api.Application.DTOs.BusinessExpence;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class BusinessExpenceProfile : Profile
    {
        public BusinessExpenceProfile()
        {
            // Entity → View DTO
            CreateMap<BusinessExpence, BusinessExpenceDto>()
                .ForMember(dest => dest.BuyingBillNo, opt => opt.MapFrom(src => src.BuyingBill != null ? src.BuyingBill.BillNo : null))
                .ForMember(dest => dest.AgencyName, opt => opt.Ignore()); // Resolved in service with Agency join

            // Create DTO → Entity
            CreateMap<CreateBusinessExpenceDto, BusinessExpence>()
                .ForMember(dest => dest.BuyingBill, opt => opt.Ignore())
                .ForMember(dest => dest.BuyingBillId, opt => opt.Ignore());

            // Update DTO → Entity (children synced separately)
            CreateMap<UpdateBusinessExpenceDto, BusinessExpence>()
                .ForMember(dest => dest.Payments, opt => opt.Ignore())
                .ForMember(dest => dest.BuyingBill, opt => opt.Ignore())
                .ForMember(dest => dest.BuyingBillId, opt => opt.Ignore());

            // Entity → List DTO
            CreateMap<BusinessExpence, BusinessExpenceListDto>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount ?? 0))
                .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => src.Payments.Sum(p => p.Amount ?? 0)))
                .ForMember(dest => dest.RemainingAmount, opt => opt.MapFrom(src =>
                    (src.TotalAmount ?? 0) - src.Payments.Sum(p => p.Amount ?? 0)))
                .ForMember(dest => dest.BuyingBillNo, opt => opt.MapFrom(src => src.BuyingBill != null ? src.BuyingBill.BillNo : null))
                .ForMember(dest => dest.AgencyName, opt => opt.Ignore()); // Resolved in service with Agency join

            // Payment child mappings
            CreateMap<BusinessExpencePayment, BusinessExpencePaymentDto>()
                .ForMember(dest => dest.PaymentAccountName, opt => opt.Ignore()); // Resolved in service
            CreateMap<CreateBusinessExpencePaymentDto, BusinessExpencePayment>();
            CreateMap<UpdateBusinessExpencePaymentDto, BusinessExpencePayment>();
        }
    }
}
