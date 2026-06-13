using AutoMapper;
using RF.WebApi.Api.Application.DTOs.AgencyPayment;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using System.Linq;

namespace RF.WebApi.Api.Application.Mappings
{
    public class AgencyPaymentProfile : Profile
    {
        public AgencyPaymentProfile()
        {
            CreateMap<AgencyPayment, AgencyPaymentDto>()
                .ForMember(dest => dest.AgencyName, opt => opt.MapFrom(src => src.Agency != null ? src.Agency.AgencyName : string.Empty))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Transactions.Sum(t => t.Amount ?? 0)));

            CreateMap<CreateAgencyPaymentDto, AgencyPayment>()
                .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions));

            CreateMap<UpdateAgencyPaymentDto, AgencyPayment>()
                .ForMember(dest => dest.Transactions, opt => opt.Ignore());

            CreateMap<AgencyPayment, AgencyPaymentListDto>()
                .ForMember(dest => dest.AgencyName, opt => opt.MapFrom(src => src.Agency != null ? src.Agency.AgencyName : string.Empty))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Transactions.Sum(t => t.Amount ?? 0)));

            CreateMap<AgencyPaymentTransaction, AgencyPaymentTransactionDto>()
                .ForMember(dest => dest.PaymentAccountName, opt => opt.MapFrom(src => src.PaymentAccount != null ? src.PaymentAccount.MethodName : string.Empty));

            CreateMap<AgencyPaymentTransactionDto, AgencyPaymentTransaction>();

            CreateMap<CreateAgencyPaymentTransactionDto, AgencyPaymentTransaction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AgencyPaymentId, opt => opt.Ignore());

            CreateMap<UpdateAgencyPaymentTransactionDto, AgencyPaymentTransaction>()
                .ForMember(dest => dest.AgencyPaymentId, opt => opt.Ignore());
        }
    }
}
