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
            CreateMap<CreateBuyingBillDto, BuyingBill>()
                .ForMember(dest => dest.Stocks, opt => opt.MapFrom(src => src.Stocks))
                .ForMember(dest => dest.Expences, opt => opt.Ignore());
            CreateMap<UpdateBuyingBillDto, BuyingBill>()
                .ForMember(dest => dest.Stocks, opt => opt.Ignore())
                .ForMember(dest => dest.Payments, opt => opt.Ignore())
                .ForMember(dest => dest.Expences, opt => opt.Ignore());

            // Buying bill list: FinalAmount = Stocks - Discount + Payments (NO expense, tracked in BusinessExpence)
            CreateMap<BuyingBill, BuyingBillListDto>()
                .ForMember(dest => dest.AgencyName, opt => opt.MapFrom(src => src.Agency != null ? src.Agency.AgencyName : string.Empty))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Stocks.Sum(i => (i.Quantity ?? 0) * (i.PurchasePrice ?? 0))))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Stocks.Sum(i => (i.Quantity ?? 0) * (i.Discount ?? 0))))
                .ForMember(dest => dest.NetAmount, opt => opt.MapFrom(src =>
                    src.Stocks.Sum(i => (i.Quantity ?? 0) * (i.PurchasePrice ?? 0)) - src.Stocks.Sum(i => (i.Quantity ?? 0) * (i.Discount ?? 0))))
                // TotalExpence mapped from BusinessExpence records
                .ForMember(dest => dest.TotalExpence, opt => opt.MapFrom(src => src.Expences.Sum(e => e.TotalAmount ?? 0)))
                .ForMember(dest => dest.TotalExpencePaid, opt => opt.MapFrom(src => src.Expences.SelectMany(e => e.Payments).Sum(p => p.Amount ?? 0)))
                .ForMember(dest => dest.TotalExpenceRemaining, opt => opt.MapFrom(src => 
                    src.Expences.Sum(e => e.TotalAmount ?? 0) - src.Expences.SelectMany(e => e.Payments).Sum(p => p.Amount ?? 0)))
                .ForMember(dest => dest.FinalAmount, opt => opt.MapFrom(src =>
                    src.Stocks.Sum(i => (i.Quantity ?? 0) * (i.PurchasePrice ?? 0)) - src.Stocks.Sum(i => (i.Quantity ?? 0) * (i.Discount ?? 0))))
                .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => src.Payments.Sum(p => p.Amount ?? 0)))
                .ForMember(dest => dest.RemainingAmount, opt => opt.MapFrom(src =>
                    (src.Stocks.Sum(i => (i.Quantity ?? 0) * (i.PurchasePrice ?? 0)) - src.Stocks.Sum(i => (i.Quantity ?? 0) * (i.Discount ?? 0)))
                    - src.Payments.Sum(p => p.Amount ?? 0)));

            CreateMap<Stock, StockDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : string.Empty));
            CreateMap<CreateStockDto, Stock>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BuyingBillId, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore()); // Date taken from bill while saving
            CreateMap<UpdateStockDto, Stock>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BuyingBillId, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore());

            CreateMap<BuyingBillPayment, BuyingBillPaymentDto>();
            CreateMap<CreateBuyingBillPaymentDto, BuyingBillPayment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BillId, opt => opt.Ignore());
            CreateMap<UpdateBuyingBillPaymentDto, BuyingBillPayment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BillId, opt => opt.Ignore());

        }
    }
}
