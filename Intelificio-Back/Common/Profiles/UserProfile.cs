using AutoMapper;
using Intelificio_Back.Features.Authentication.Commands.Signup;
using Intelificio_Back.Models;


namespace AZ_204.Common.Profilers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            _ = CreateMap<SignUpCommand, User>().AfterMap((x, y) =>
            {
                y.UserName = x.Email;
            });
        }
    }
}
