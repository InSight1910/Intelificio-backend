using Backend.Common.Response;
using Backend.Models;
using MediatR;

namespace Backend.Features.Notification.Commands.SingleUserSignUpSummary
{
    public class SingleUserSignUpSummaryCommand : IRequest<Result>
    {
        public int CreatorID { get; set; } // ID del administrador o quien creo la cuenta en sistema.
        public required User user { get; set; } // Usuario Creado
        public int CommunityID { get; set; } // ID de Comunidad
    }
}
