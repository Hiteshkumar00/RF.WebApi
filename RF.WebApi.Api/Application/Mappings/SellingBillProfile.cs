using System.Linq;
using AutoMapper;
using RF.WebApi.Api.Application.DTOs.SellingBill;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class SellingBillProfile : Profile
    {
        public SellingBillProfile()
        {
            // Main Bill mappings
            CreateMap<SellingBill, SellingBillDto>();
            CreateMap<CreateSellingBillDto, SellingBill>();
            CreateMap<UpdateSellingBillDto, SellingBill>()
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ForMember(dest => dest.Payments, opt => opt.Ignore());

            // List projection with mathematical rollups
            CreateMap<SellingBill, SellingBillListDto>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Items.Sum(i => (i.Quantity ?? 0) * (i.Price ?? 0))))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount ?? 0))
                .ForMember(dest => dest.NetAmount, opt => opt.MapFrom(src => 
                    src.Items.Sum(i => (i.Quantity ?? 0) * (i.Price ?? 0)) - (src.Discount ?? 0)))
                .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => src.Payments.Sum(p => p.Amount ?? 0)))
                .ForMember(dest => dest.RemainingAmount, opt => opt.MapFrom(src => 
                    (src.Items.Sum(i => (i.Quantity ?? 0) * (i.Price ?? 0)) - (src.Discount ?? 0)) // Net Amount
                    - src.Payments.Sum(p => p.Amount ?? 0)                                       // - Paid
                ));

            // Item mappings (EF handles the 1-to-1 Warrenty assignment automatically because of the identical nesting structure)
            CreateMap<SellingBillItem, SellingBillItemDto>();
            CreateMap<CreateSellingBillItemDto, SellingBillItem>();
            CreateMap<UpdateSellingBillItemDto, SellingBillItem>();

            // Warranty mappings
            CreateMap<SellingItemWarrenty, SellingItemWarrentyDto>();
            CreateMap<CreateSellingItemWarrentyDto, SellingItemWarrenty>();
            CreateMap<UpdateSellingItemWarrentyDto, SellingItemWarrenty>();

            // Payment mappings
            CreateMap<SellingBillPayment, SellingBillPaymentDto>();
            CreateMap<CreateSellingBillPaymentDto, SellingBillPayment>();
            CreateMap<UpdateSellingBillPaymentDto, SellingBillPayment>();
        }
    }
}
