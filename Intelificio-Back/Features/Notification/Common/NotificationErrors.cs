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
        public static Error TemplateNotFoundOnPackage = new()
        {
            Code = "Notification.Package.TemplteIdNotFoundOnPackage",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error PackageNotFound = new()
        {
            Code = "Notification.Package.PackageNotFound",
            Message = "No se ha podido encontrar el Package consultado."
        };
        public static Error TemplateIdIsNullOnPackage = new()
        {
            Code = "Notification.Package.PackageNotFound",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };
    }
}
