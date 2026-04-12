using AutoMapper;
using RF.WebApi.Api.Application.DTOs.User;
using RF.WebApi.Api.Infrastructure.Data.Tables;


namespace RF.WebApi.Api.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());


            CreateMap<User, UserDto>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account != null ? src.Account.ProfileName : string.Empty));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
