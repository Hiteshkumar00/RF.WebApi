using AutoMapper;
using RF.WebApi.Api.Application.DTOs.BuyingBill;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class BuyingBillProfile : Profile
    {
        public BuyingBillProfile()
        {
            CreateMap<BuyingBill, BuyingBillDto>();
            CreateMap<CreateBuyingBillDto, BuyingBill>();
            CreateMap<UpdateBuyingBillDto, BuyingBill>()
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ForMember(dest => dest.Payments, opt => opt.Ignore())
                .ForMember(dest => dest.Expences, opt => opt.Ignore());

            CreateMap<BuyingBill, BuyingBillListDto>()
                .ForMember(dest => dest.AgencyName, opt => opt.MapFrom(src => src.Agency != null ? src.Agency.AgencyName : string.Empty))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Items.Sum(i => (i.Quantity ?? 0) * (i.Price ?? 0))))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount ?? 0))
                .ForMember(dest => dest.NetAmount, opt => opt.MapFrom(src => 
                    src.Items.Sum(i => (i.Quantity ?? 0) * (i.Price ?? 0)) - (src.Discount ?? 0)))
                .ForMember(dest => dest.TotalExpence, opt => opt.MapFrom(src => src.Expences.Sum(e => e.Amount ?? 0)))
                .ForMember(dest => dest.FinalAmount, opt => opt.MapFrom(src => 
                    (src.Items.Sum(i => (i.Quantity ?? 0) * (i.Price ?? 0)) - (src.Discount ?? 0)) // Net Amount
                    + src.Expences.Sum(e => e.Amount ?? 0)                                       // + Expenses
                ))
                .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => src.Payments.Sum(p => p.Amount ?? 0)))
                .ForMember(dest => dest.RemainingAmount, opt => opt.MapFrom(src => 
                    (src.Items.Sum(i => (i.Quantity ?? 0) * (i.Price ?? 0)) - (src.Discount ?? 0)) // Net Amount
                    + src.Expences.Sum(e => e.Amount ?? 0)                                       // + Expenses
                    - src.Payments.Sum(p => p.Amount ?? 0)                                       // - Paid
                ));

            CreateMap<BuyingBillItem, BuyingBillItemDto>();
            CreateMap<CreateBuyingBillItemDto, BuyingBillItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BillId, opt => opt.Ignore());
            CreateMap<UpdateBuyingBillItemDto, BuyingBillItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BillId, opt => opt.Ignore());

            CreateMap<BuyingBillPayment, BuyingBillPaymentDto>();
            CreateMap<CreateBuyingBillPaymentDto, BuyingBillPayment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BillId, opt => opt.Ignore());
            CreateMap<UpdateBuyingBillPaymentDto, BuyingBillPayment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BillId, opt => opt.Ignore());

            CreateMap<BuyingBillExpence, BuyingBillExpenceDto>();
            CreateMap<CreateBuyingBillExpenceDto, BuyingBillExpence>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BillId, opt => opt.Ignore());
            CreateMap<UpdateBuyingBillExpenceDto, BuyingBillExpence>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BillId, opt => opt.Ignore());
        }
    }
}
