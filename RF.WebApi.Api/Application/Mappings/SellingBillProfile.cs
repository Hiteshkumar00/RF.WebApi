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
            CreateMap<SellingBill, SellingBillDto>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Items.Sum(i => (i.Quantity ?? 0) * (i.Price ?? 0))))
                .ForMember(dest => dest.NetAmount, opt => opt.MapFrom(src => 
                    src.Items.Sum(i => (i.Quantity ?? 0) * (i.Price ?? 0)) - src.Items.Sum(i => (i.Quantity ?? 0) * (i.Discount ?? 0))));
            
            CreateMap<CreateSellingBillDto, SellingBill>();
            CreateMap<UpdateSellingBillDto, SellingBill>()
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ForMember(dest => dest.Payments, opt => opt.Ignore());

            // List projection with mathematical rollups
            CreateMap<SellingBill, SellingBillListDto>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Items.Sum(i => (i.Quantity ?? 0) * (i.Price ?? 0))))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Items.Sum(i => (i.Quantity ?? 0) * (i.Discount ?? 0))))
                .ForMember(dest => dest.NetAmount, opt => opt.MapFrom(src => 
                    src.Items.Sum(i => (i.Quantity ?? 0) * (i.Price ?? 0)) - src.Items.Sum(i => (i.Quantity ?? 0) * (i.Discount ?? 0))))
                .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => src.Payments.Sum(p => p.Amount ?? 0)))
                .ForMember(dest => dest.RemainingAmount, opt => opt.MapFrom(src => 
                    (src.Items.Sum(i => (i.Quantity ?? 0) * (i.Price ?? 0)) - src.Items.Sum(i => (i.Quantity ?? 0) * (i.Discount ?? 0))) // Net Amount
                    - src.Payments.Sum(p => p.Amount ?? 0)                                       // - Paid
                ));

            // Item mappings
            CreateMap<SellingBillItem, SellingBillItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : string.Empty));
            CreateMap<CreateSellingBillItemDto, SellingBillItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BillId, opt => opt.Ignore());
            CreateMap<UpdateSellingBillItemDto, SellingBillItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BillId, opt => opt.Ignore());

            // Payment mappings
            CreateMap<SellingBillPayment, SellingBillPaymentDto>();
            CreateMap<SellingBillPaymentDto, SellingBillPayment>();
            CreateMap<CreateSellingBillPaymentDto, SellingBillPayment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BillId, opt => opt.Ignore());
            CreateMap<UpdateSellingBillPaymentDto, SellingBillPayment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BillId, opt => opt.Ignore());
        }
    }
}
