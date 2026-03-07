using AutoMapper;
using RF.WebApi.Api.Application.DTOs.Contribution;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class ContributionProfile : Profile
    {
        public ContributionProfile()
        {
            // AddContribution Mappings
            CreateMap<AddContribution, AddContributionDto>().ReverseMap();
            CreateMap<CreateAddContributionDto, AddContribution>();
            CreateMap<UpdateAddContributionDto, AddContribution>();
            CreateMap<AddContribution, AddContributionListDto>()
                .ForMember(dest => dest.AccountPersonName, opt => opt.MapFrom(src => src.AccountPerson != null ? src.AccountPerson.Name : string.Empty))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Payments.Sum(p => p.Amount ?? 0)));

            // AddContributionPayment Mappings
            CreateMap<AddContributionPayment, AddContributionPaymentDto>().ReverseMap();
            CreateMap<CreateAddContributionPaymentDto, AddContributionPayment>();
            CreateMap<UpdateAddContributionPaymentDto, AddContributionPayment>();

            // RemoveContribution Mappings
            CreateMap<RemoveContribution, RemoveContributionDto>().ReverseMap();
            CreateMap<CreateRemoveContributionDto, RemoveContribution>();
            CreateMap<UpdateRemoveContributionDto, RemoveContribution>();
            CreateMap<RemoveContribution, RemoveContributionListDto>()
                .ForMember(dest => dest.AccountPersonName, opt => opt.MapFrom(src => src.AccountPerson != null ? src.AccountPerson.Name : string.Empty))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Payments.Sum(p => p.Amount ?? 0)));

            // RemoveContributionPayment Mappings
            CreateMap<RemoveContributionPayment, RemoveContributionPaymentDto>().ReverseMap();
            CreateMap<CreateRemoveContributionPaymentDto, RemoveContributionPayment>();
            CreateMap<UpdateRemoveContributionPaymentDto, RemoveContributionPayment>();
        }
    }
}
