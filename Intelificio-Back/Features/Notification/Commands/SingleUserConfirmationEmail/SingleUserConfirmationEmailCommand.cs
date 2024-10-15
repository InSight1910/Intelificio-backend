using Backend.Common.Response;
using Backend.Models;
using MediatR;

namespace Backend.Features.Notification.Commands.ConfirmEmail
{
    public class SingleUserConfirmationEmailCommand : IRequest<Result>
    {
        public required List<User> Users { get; set; } // Lista de usuarios para enviar correo

        public int CommunityID { get; set; } // ID de Comunidad

    }
}
