using AutoMapper;
using Backend.Common.Response;
using Backend.Common.Security;
using Backend.Features.Authentication.Common;
using Backend.Features.Community.Commands.AddUser;
using Backend.Features.Notification.Commands.ConfirmEmail;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Backend.Features.Authentication.Commands.Signup
{
    public class SignUpCommandHandler(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, IMediator mediator) : IRequestHandler<SignUpCommand, List<Result>>
    {
        public async Task<List<Result>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {

            var results = new List<Result>();

            if (request.User != null)
            {
                var result = await DoSignUp(request.User, request.CommunityID, request.IsMassive);
                results.Add(result);
            }
            else if (request.Users != null)
            {
                foreach (var user in request.Users)
                {
                    var result = await DoSignUp(user, request.CommunityID, request.IsMassive);
                    results.Add(result);
                }
            }

            return results;
        }
    

        private async Task<Result> DoSignUp(UserObject request, int CommunityID, Boolean IsMassive)
        {
            var userExist = await userManager.FindByEmailAsync(request.Email);

            if (userExist != null) return Result.Failure(AuthenticationErrors.AlreadyCreatedEmail(request.Email));

            var userExistRut = await userManager.Users.AnyAsync(x => x.Rut == request.Rut);

            if (userExistRut) return Result.Failure(AuthenticationErrors.AlreadyCreatedRut(request.Rut));

            var user = mapper.Map<User>(request);

            var roleExist = await roleManager.FindByNameAsync(request.Role);
            if (roleExist == null) return Result.Failure(AuthenticationErrors.RoleNotFound);

            var result = await userManager.CreateAsync(user, PasswordGenerator.GenerateSecurePassword(14));

           
            if (result.Errors.Any())
            {
                return Result.Failure(AuthenticationErrors.InvalidParameters(result.Errors.Select(e => e.Description).ToList()));
            }

            _ = await userManager.AddToRoleAsync(user, roleExist.Name!);

            var addUserCommunityCommand = new AddUserCommunityCommand
            {
                User = new AddUserObject
                {
                    CommunityId = CommunityID,
                    UserId = user.Id
                }
            };

            var addUserCommunityCommandResult = await mediator.Send(addUserCommunityCommand);
            if (addUserCommunityCommandResult.IsFailure) return Result.Failure(addUserCommunityCommandResult.Errors);

            if (!IsMassive)
            {
                
                var confirmEmailCommand = new SingleUserConfirmationEmailCommand
                {
                    Users = new List<User> { user },
                    CommunityID = CommunityID
                };
                var confirmEmailResult = await mediator.Send(confirmEmailCommand);
                if (confirmEmailResult.IsFailure) return Result.Failure(confirmEmailResult.Errors);
            }

            return Result.Success();
        }

    }
}
