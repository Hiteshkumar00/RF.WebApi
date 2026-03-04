using AutoMapper;
using RF.WebApi.Api.Application.DTOs.AccountPerson;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class AccountPersonProfile : Profile
    {
        public AccountPersonProfile()
        {
            CreateMap<AccountPerson, AccountPersonDto>().ReverseMap();
            CreateMap<CreateAccountPersonDto, AccountPerson>();
            CreateMap<UpdateAccountPersonDto, AccountPerson>();
        }
    }
}
