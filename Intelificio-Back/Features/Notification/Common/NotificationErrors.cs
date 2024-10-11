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

        //Errores de Package
        public static Error EmailNotSentOnPackage = new()
        {
            Code = "Notification.Package.EmailNotSentOnPackage",
            Message = "No se ha podido enviar el correo."
        };
        public static Error PackageNotFound = new()
        {
            Code = "Notification.Package.PackageNotFound",
            Message = "No se ha podido encontrar el Package consultado."
        };
        public static Error TemplateNotFoundOnPackage = new()
        {
            Code = "Notification.Package.TemplteIdNotFoundOnPackage",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnPackage = new()
        {
            Code = "Notification.Package.PackageNotFound",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };

        //Errores de SuccessfulReservation
        public static Error EmailNotSentOnSuccessfulReservation = new()
        {
            Code = "Notification.SuccessfulReservation.EmailNotSentOnSuccessfulReservation",
            Message = "No se ha podido enviar el correo."
        };
        public static Error ReservationNotFoundOnSuccessfulReservation = new()
        {
            Code = "Notification.SuccessfulReservation.ReservationNotFoundOnSuccessfulReservation",
            Message = "No se ha podido encontrar la reservación."
        };
        public static Error TemplateNotFoundOnSuccessfulReservation = new()
        {
            Code = "Notification.SuccessfulReservation.TemplateNotFoundOnSuccessfulReservation",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnSuccessfulReservation = new()
        {
            Code = "Notification.SuccessfulReservation.TemplateIdIsNullOnSuccessfulReservation",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };

        //Errores de ReservationCancellation
        public static Error EmailNotSentOnReservationCancellation = new()
        {
            Code = "Notification.ReservationCancellation.EmailNotSentOnReservationCancellation",
            Message = "No se ha podido enviar el correo."
        };
        public static Error ReservationNotFoundOnReservationCancellation = new()
        {
            Code = "Notification.ReservationCancellation.ReservationNotFoundOnReservationCancellation",
            Message = "No se ha podido encontrar la reservación."
        };
        public static Error TemplateNotFoundOnReservationCancellation = new()
        {
            Code = "Notification.ReservationCancellation.TemplateNotFoundOnReservationCancellation",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnReservationCancellation = new()
        {
            Code = "Notification.ReservationCancellation.TemplateIdIsNullOnReservationCancellation",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };

        //Errores de Maintenance
        public static Error CommonSpaceNotFound = new()
        {
            Code = "Notification.Maintenance.CommonSpaceNotFound",
            Message = "No se ha podido encontrar el espacio común."
        };
        public static Error TemplateNotFoundOnMaintenance = new()
        {
            Code = "Notification.Maintenance.TemplateNotFoundOnReservationCancellation",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnMaintenance = new()
        {
            Code = "Notification.Maintenance.TemplateIdIsNullOnReservationCancellation",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };

        // Errores en SingleUserConfirmationEmail
        public static Error TemplateNotFoundOnSingleUserConfirmationEmail = new()
        {
            Code = "Notification.SingleUserConfirmationEmail.TemplateNotFoundOnSingleUserConfirmationEmail",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnSingleUserConfirmationEmail = new()
        {
            Code = "Notification.SingleUserConfirmationEmail.TemplateIdIsNullOnSingleUserConfirmationEmail",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };
        public static Error TemplateNotCreatedOnSingleUserConfirmationEmail = new()
        {
            Code = "Notification.SingleUserConfirmationEmail.TemplateNotCreatedOnSingleUserConfirmationEmail",
            Message = "No se logró armar el template para enviar la confirmación de correo."
        };
        public static Error RecipientsNotCreatedOnSingleUserConfirmationEmail = new()
        {
            Code = "Notification.SingleUserConfirmationEmail.RecipientsNotCreatedOnSingleUserConfirmationEmail",
            Message = "No se logró armar la lista de destinatarios para enviar la confirmación de correo."
        };
        public static Error EmailNotSentOnSingleUserConfirmationEmail = new()
        {
            Code = "Notification.SingleUserConfirmationEmail.EmailNotSentOnSingleUserConfirmationEmail",
            Message = "No se ha podido enviar el correo."
        };
        public static Error CommunityNotfoundOnSingleUserConfirmationEmail = new()
        {
            Code = "Notification.SingleUserConfirmationEmail.CommunityNotfoundOnSingleUserConfirmationEmail",
            Message = "No se encontró la comunidad."
        };


        // Errores en SingleUserConfirmationEmail
        public static Error TemplateNotFoundOnMassUserConfirmationEmail = new()
        {
            Code = "Notification.MassUserConfirmationEmail.TemplateNotFoundOnMassUserConfirmationEmail",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnMassUserConfirmationEmail = new()
        {
            Code = "Notification.MassUserConfirmationEmail.TemplateIdIsNullOnMassUserConfirmationEmail",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };
        public static Error TemplateNotCreatedOnMassUserConfirmationEmail = new()
        {
            Code = "Notification.MassUserConfirmationEmail.TemplateNotCreatedOnMassUserConfirmationEmail",
            Message = "No se logró armar el template para enviar la confirmación de correo."
        };
        public static Error RecipientsNotCreatedOnMassUserConfirmationEmail = new()
        {
            Code = "Notification.MassUserConfirmationEmail.RecipientsNotCreatedOnMassUserConfirmationEmail",
            Message = "No se logró armar la lista de destinatarios para enviar la confirmación de correo."
        };
        public static Error EmailNotSentOnMassUserConfirmationEmail = new()
        {
            Code = "Notification.MassUserConfirmationEmail.EmailNotSentOnMassUserConfirmationEmail",
            Message = "No se ha podido enviar el correo."
        };
        public static Error CommunityNotfoundOnMassUserConfirmationEmail = new()
        {
            Code = "Notification.MassUserConfirmationEmail.CommunityNotfoundOnMassUserConfirmationEmail",
            Message = "No se encontró la comunidad."
        };

     


        //Errores de MaintenanceCancellation
        public static Error CommonSpaceNotFoundOnMaintenanceCancellation = new()
        {
            Code = "Notification.MaintenanceCancellation.CommonSpaceNotFoundOnMaintenanceCancellation",
            Message = "No se ha podido encontrar el espacio común."
        };
        public static Error CommunityNotFoundOnMaintenanceCancellation = new()
        {
            Code = "Notification.MaintenanceCancellation.CommunityNotFoundOnMaintenanceCancellation",
            Message = "No se ha podido completar el template para enviar correo."
        };
        public static Error TemplateNotFoundOnMaintenanceCancellation = new()
        {
            Code = "Notification.MaintenanceCancellation.TemplateNotFoundOnMaintenanceCancellation",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnMaintenanceCancellation = new()
        {
            Code = "Notification.MaintenanceCancellation.TemplateIdIsNullOnMaintenanceCancellation",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };
        public static Error TemplateNotCreatedOnMaintenanceCancellation = new()
        {
            Code = "Notification.MaintenanceCancellation.TemplateNotCreatedOnMaintenanceCancellation",
            Message = "El templates no puede ser null para enviar el termino de mantención."
        };
        public static Error EmailNotSentOnMaintenanceCancellation = new()
        {
            Code = "Notification.MaintenanceCancellation.EmailNotSentOnMaintenanceCancellation",
            Message = "No se ha podido enviar el correo."
        };


        //Errores de SimpleMessage

        public static Error TemplateNotFoundOnSimpleMessage = new()
        {
            Code = "Notification.SimpleMessage.TemplateNotFoundOnSimpleMessage",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnSimpleMessage = new()
        {
            Code = "Notification.SimpleMessage.TemplateIdIsNullOnSimpleMessage",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };

        //Errores de SingleUserSignUpSummary
        public static Error EmailNotSentOnSingleUserSignUpSummary = new()
        {
            Code = "Notification.SingleUserSignUpSummary.EmailNotSentOnSingleUserSignUpSummary",
            Message = "No se ha podido enviar el correo."
        };
        public static Error AdminUserNotExistOnSingleUserSignUpSummary = new()
        {
            Code = "Notification.SingleUserSignUpSummary.AdminUserNotExistOnSingleUserSignUpSummary",
            Message = "No se logró ubicar al administrador informado."
        };
        public static Error CommunityNotFoundOnSingleUserSignUpSummary = new()
        {
            Code = "Notification.SingleUserSignUpSummary.CommunityNotFoundOnSingleUserSignUpSummary",
            Message = "No existe la comunidad consultada."
        };
        public static Error TemplateNotFoundOnSingleUserSignUpSummary = new()
        {
            Code = "Notification.SingleUserSignUpSummary.TemplateNotFoundOnSingleUserSignUpSummary",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnSingleUserSignUpSummary = new()
        {
            Code = "Notification.SingleUserSignUpSummary.TemplateIdIsNullOnSingleUserSignUpSummary",
        };

        //Errores de MassUserSignUpSummary
        public static Error EmailNotSentOnMassUserSignUpSummary = new()
        {
            Code = "Notification.MassUserSignUpSummary.EmailNotSentOnMassUserSignUpSummary",
            Message = "No se ha podido enviar el correo."
        };
        public static Error AdminUserNotExistOnMassUserSignUpSummary = new()
        {
            Code = "Notification.MassUserSignUpSummary.AdminUserNotExistOnMassUserSignUpSummary",
            Message = "No se logró ubicar al administrador informado."
        };
        public static Error CommunityNotFoundOnMassUserSignUpSummary = new()
        {
            Code = "Notification.MassUserSignUpSummary.CommunityNotFoundOnMassUserSignUpSummary",
            Message = "No existe la comunidad consultada."
        };
        public static Error TemplateNotFoundOnMassUserSignUpSummary = new()
        {
            Code = "Notification.MassUserSignUpSummary.TemplateNotFoundOnMassUserSignUpSummary",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnMassUserSignUpSummary = new()
        {
            Code = "Notification.MassUserSignUpSummary.TemplateIdIsNullOnMassUserSignUpSummary",
        };



        //Errores de CommonExpenses
        public static Error TemplateNotCreated = new()
        {
            Code = "Notification.CommonExpenses.TemplateNotCreated",
            Message = "El templates no puede ser null para enviar las notificaciones de Gastos común"
        };
        public static Error TemplateNotFoundOnCommonExpenses = new()
        {
            Code = "Notification.CommonExpenses.TemplateNotFoundOnCommonExpenses",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnCommonExpenses = new()
        {
            Code = "Notification.CommonExpenses.TemplateIdIsNullOnCommonExpenses",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };

        // Errores de ReservationConfirmation
        public static Error ReservationNotFound = new()
        {
            Code = "Notification.ReservationConfirmation.ReservationNotFound",
            Message = "No se logró ubicar la reserva consultada."
        };
        public static Error TemplateNotFoundOnReservationConfirmation = new()
        {
            Code = "Notification.ReservationConfirmation.TemplateNotFoundOnCommonExpenses",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnReservationConfirmation = new()
        {
            Code = "Notification.ReservationConfirmation.TemplateIdIsNullOnCommonExpenses",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };
        public static Error TemplateNotCreatedOnReservationConfirmation = new()
        {
            Code = "Notification.ReservationConfirmation.TemplateNotCreatedOnReservationConfirmation",
            Message = "El templates no puede ser null para enviar confirmación de reserva."
        };
        public static Error EmailNotSentOnReservationConfirmation = new()
        {
            Code = "Notification.ReservationConfirmation.EmailNotSentOnReservationConfirmation",
            Message = "No se pudo enviar el correo de confirmación de reserva"
        };

    }
}
