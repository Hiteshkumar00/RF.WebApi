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
        }
    }
}
