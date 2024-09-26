using Backend.Common.Response;

namespace Backend.Features.Notification.Common
{
    public class NotificationErrors
    {
        public static Error EmailNotSent = new()
        {
            Code = "Notification.SingleMessage.EmailNotSent",
            Message = "No se ha podido enviar el correo."
        };
    }
}
