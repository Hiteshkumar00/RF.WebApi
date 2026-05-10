using AutoMapper;
using RF.WebApi.Api.Application.DTOs.PaymentAccount;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class PaymentAccountProfile : Profile
    {
        public PaymentAccountProfile()
        {
            CreateMap<PaymentAccount, PaymentAccountDto>();
            CreateMap<CreatePaymentAccountDto, PaymentAccount>();
            CreateMap<UpdatePaymentAccountDto, PaymentAccount>();

            CreateMap<PaymentTransfer, PaymentTransferDto>()
                .ForMember(dest => dest.FromPaymentAccountName, opt => opt.MapFrom(src => src.FromPaymentAccount != null ? src.FromPaymentAccount.MethodName : ""))
                .ForMember(dest => dest.ToPaymentAccountName, opt => opt.MapFrom(src => src.ToPaymentAccount != null ? src.ToPaymentAccount.MethodName : ""));
            CreateMap<CreatePaymentTransferDto, PaymentTransfer>();
            CreateMap<UpdatePaymentTransferDto, PaymentTransfer>();
        }
    }
}
