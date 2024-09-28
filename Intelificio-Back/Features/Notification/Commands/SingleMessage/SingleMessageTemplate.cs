using static Mysqlx.Crud.Order.Types;

namespace Backend.Features.Notification.Commands.SingleMessage
{
    public class SingleMessageTemplate
    {
        public required string Subject { get; set; } // Asunto
        public required string PreHeader { get; set; } // PreHeader
        public required string Title { get; set; } // Titulo del correo en el cuerpo
        public required string CommunityName { get; set; } // Nombre de comunidad | Ejemplo: Las Brisas de San Juan
        public required string Message { get; set; } // Mensaje del correo que irá en el cuerpo del mismo
        public required string SenderAddress { get; set; } // Dirección de la comunidad | Ejemplo: Tangamandapio #243, La Florida

    }
}
