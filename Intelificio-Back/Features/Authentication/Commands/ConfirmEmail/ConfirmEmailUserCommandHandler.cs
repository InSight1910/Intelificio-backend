using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Backend.Features.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailUserCommandHandler : IRequestHandler<ConfirmEmailUserCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ConfirmEmailUserCommandHandler> _logger;
        private readonly IMapper _mapper;

        public ConfirmEmailUserCommandHandler(UserManager<User> userManager, ILogger<ConfirmEmailUserCommandHandler> logger, IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(ConfirmEmailUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user == null) return Result.Failure(AuthenticationErrors.USerNotSendOnConfirmEmail);
            if(user.EmailConfirmed) return Result.Failure(AuthenticationErrors.UserAlreadyConfirmThisEmailOnOnConfirmEmail);


            var emailConfirmationResult = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (!emailConfirmationResult.Succeeded)
            {
                return Result.Failure(AuthenticationErrors.InvalidToken);
            }

            // Generar un nuevo token para cambiar la contraseña
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Mapeamos el User al ConfirmEmailUserCommandResponse usando AutoMapper
            var response = _mapper.Map<ConfirmEmailUserCommandResponse>(user);
            response.Token = passwordResetToken; // Asignamos el token manualmente


            return Result.WithResponse(new ResponseData()
            {
                Data = response
            });
        }
    }
}