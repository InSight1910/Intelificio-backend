using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.Features.Authentication.Commands.Signup
{
    public class SignUpCommandHandler(UserManager<User> userManager, IMapper mapper) : IRequestHandler<SignUpCommand, Result>
    {
        public async Task<Result> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var userExist = await userManager.FindByEmailAsync(request.Email);
            if (userExist != null) return Result.Failure(AuthenticationErrors.AlreadyCreated);

            var user = mapper.Map<User>(request);

            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Errors.Any())
            {
                var errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return Result.Failure(AuthenticationErrors.InvalidParameters(errors));
            }

            return Result.Success();
        }

    }
}
