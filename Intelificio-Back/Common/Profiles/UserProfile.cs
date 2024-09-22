using AutoMapper;
using Backend.Features.Authentication.Commands.Signup;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            _ = CreateMap<UserObject, User>()
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .AfterMap((x, y) =>
            {
                y.UserName = x.Email;
            });
        }
    }
}
