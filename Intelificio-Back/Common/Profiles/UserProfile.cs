using AutoMapper;
using Backend.Features.Authentication.Commands.Signup;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            _ = CreateMap<UserObject, User>().AfterMap((x, y) =>
            {
                y.UserName = x.Email;
            });
        }
    }
}
