using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Backend.Features.Authentication.Commands.ChangePasswordOne
{
    public class ChangePasswordOneCommandHandler : IRequestHandler<ChangePasswordOneCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly SendMail _sendMail;

        public ChangePasswordOneCommandHandler(UserManager<User> userManager, SendMail sendMail)
        {
            _userManager = userManager;
            _sendMail = sendMail;
        }

        public async Task<Result> Handle(ChangePasswordOneCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user == null) return Result.Failure(AuthenticationErrors.UserNotFound);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var message = @"
             <body>
                <div class=""email-container"">
                    <h2>Solicitud de Restablecimiento de Contraseña</h2>
                    <p>Hola,</p>
                    <p>Hemos recibido una solicitud para restablecer tu contraseña. Si no has solicitado este cambio, puedes ignorar este correo electrónico. De lo contrario, haz clic en el botón de abajo para restablecer tu contraseña:</p>

                    <a href=""[[RESET_LINK]]"" class=""btn-reset"">Restablecer Contraseña</a>

                    <p>Si el botón no funciona, puedes copiar y pegar el siguiente enlace en tu navegador:</p>
                    <p>[[RESET_LINK]]</p>

                    <p>Este código es válido por los próximos 30 minutos.</p>

                    <footer>
                        <p>Si no solicitaste un cambio de contraseña, por favor contacta a nuestro equipo de soporte.</p>
                    </footer>
                </div>
            </body>
            ";

            var result = await _sendMail.SendEmailAsync(user.Email!, "Restablecer Contraseña",
                message.Replace("[[RESET_LINK]]", $"http://localhost:4200/change-password?email={user.Email}&token={WebUtility.UrlEncode(token)}")
            );

            if (!result.IsSuccessStatusCode) return Result.Failure(AuthenticationErrors.EmailNotSent);
            return Result.Success();
        }
    }
}
