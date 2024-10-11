using Backend.Common.Response;
using Backend.Models;
using MediatR;

namespace Backend.Features.Notification.Commands.MassUserSignUpSummary
{
    public class MassUserSignUpSummaryCommand: IRequest<Result>
    {
        public int CreatorID { get; set; } // ID del administrador o quien creo la cuenta en sistema.
        public int CommunityID { get; set; } // ID de Comunidad
        public required string TotalCreados { get; set; } // Total de usuarios Creados
        public required string TotalEnviados { get; set; } // Total de usuarios enviados
        public required string TotalErrores { get; set; } // Total de usuarios no creados
    }
}
