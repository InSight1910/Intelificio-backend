using Backend.Common.Response;

namespace Backend.Features.Notification.Common
{
    public class NotificationErrors
    {
        public static readonly Error EmailNotSent = new()
        {
            Code = "Notification.SingleMessage.EmailNotSent",
            Message = "No se ha podido enviar el correo."
        };

        //Errores de Package
        public static readonly Error EmailNotSentOnPackage = new()
        {
            Code = "Notification.Package.EmailNotSentOnPackage",
            Message = "No se ha podido enviar el correo."
        };
        public static readonly Error PackageNotFound = new()
        {
            Code = "Notification.Package.PackageNotFound",
            Message = "No se ha podido encontrar el Package consultado."
        };
        public static readonly Error TemplateNotFoundOnPackage = new()
        {
            Code = "Notification.Package.TemplteIdNotFoundOnPackage",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnPackage = new()
        {
            Code = "Notification.Package.PackageNotFound",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };
        public static readonly Error LimmitNotificationSentOnPackage = new()
        {
            Code = "Notification.Package.LimmitNotificationSentOnPackage",
            Message = "Se ha alcanzado el limite de notificaciones permitidas"
        };

        //Errores de PackageDelivered 
        public static readonly Error PackageNotFoundOnPackageDelivered = new()
        {
            Code = "Notification.PackageDelivered.PackageNotFoundOnPackageDelivered",
            Message = "No se ha podido encontrar el packete consultado el correo."
        };
        public static readonly Error EmailNotSentOnPackageDelivered = new()
        {
            Code = "Notification.PackageDelivered.EmailNotSentOnPackageDelivered",
            Message = "No se ha podido enviar el correo."
        };
        public static readonly Error DeliveredUserNotfoundOnPackageDelivered = new()
        {
            Code = "Notification.PackageDelivered.DeliveredUserNotfoundOnPackageDelivered",
            Message = "No se ha podido encontrar el Package consultado."
        };
        public static readonly Error TemplateNotFoundOnPackageDelivered = new()
        {
            Code = "Notification.PackageDelivered.TemplateNotFoundOnPackageDelivered",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnPackageDelivered = new()
        {
            Code = "Notification.PackageDelivered.TemplateIdIsNullOnPackageDelivered",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };


        //Errores de SuccessfulReservation
        public static readonly Error EmailNotSentOnSuccessfulReservation = new()
        {
            Code = "Notification.SuccessfulReservation.EmailNotSentOnSuccessfulReservation",
            Message = "No se ha podido enviar el correo."
        };
        public static readonly Error ReservationNotFoundOnSuccessfulReservation = new()
        {
            Code = "Notification.SuccessfulReservation.ReservationNotFoundOnSuccessfulReservation",
            Message = "No se ha podido encontrar la reservación."
        };
        public static readonly Error TemplateNotFoundOnSuccessfulReservation = new()
        {
            Code = "Notification.SuccessfulReservation.TemplateNotFoundOnSuccessfulReservation",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnSuccessfulReservation = new()
        {
            Code = "Notification.SuccessfulReservation.TemplateIdIsNullOnSuccessfulReservation",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };

        //Errores de ReservationCancellation
        public static readonly Error EmailNotSentOnReservationCancellation = new()
        {
            Code = "Notification.ReservationCancellation.EmailNotSentOnReservationCancellation",
            Message = "No se ha podido enviar el correo."
        };
        public static readonly Error ReservationNotFoundOnReservationCancellation = new()
        {
            Code = "Notification.ReservationCancellation.ReservationNotFoundOnReservationCancellation",
            Message = "No se ha podido encontrar la reservación."
        };
        public static readonly Error TemplateNotFoundOnReservationCancellation = new()
        {
            Code = "Notification.ReservationCancellation.TemplateNotFoundOnReservationCancellation",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnReservationCancellation = new()
        {
            Code = "Notification.ReservationCancellation.TemplateIdIsNullOnReservationCancellation",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };

        //Errores de Maintenance
        public static readonly Error CommonSpaceNotFound = new()
        {
            Code = "Notification.Maintenance.CommonSpaceNotFound",
            Message = "No se ha podido encontrar el espacio común."
        };
        public static readonly Error TemplateNotFoundOnMaintenance = new()
        {
            Code = "Notification.Maintenance.TemplateNotFoundOnReservationCancellation",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnMaintenance = new()
        {
            Code = "Notification.Maintenance.TemplateIdIsNullOnReservationCancellation",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };
        public static readonly Error CommunityDataIsNull = new()
        {
            Code = "Notification.Maintenance.CommunityDataIsNull",
            Message = "No se logró encontrar información de la comunidad."
        };

        // Errores en SingleUserConfirmationEmail
        public static readonly Error TemplateNotFoundOnSingleUserConfirmationEmail = new()
        {
            Code = "Notification.SingleUserConfirmationEmail.TemplateNotFoundOnSingleUserConfirmationEmail",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnSingleUserConfirmationEmail = new()
        {
            Code = "Notification.SingleUserConfirmationEmail.TemplateIdIsNullOnSingleUserConfirmationEmail",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };
        public static readonly Error TemplateNotCreatedOnSingleUserConfirmationEmail = new()
        {
            Code = "Notification.SingleUserConfirmationEmail.TemplateNotCreatedOnSingleUserConfirmationEmail",
            Message = "No se logró armar el template para enviar la confirmación de correo."
        };
        public static readonly Error RecipientsNotCreatedOnSingleUserConfirmationEmail = new()
        {
            Code = "Notification.SingleUserConfirmationEmail.RecipientsNotCreatedOnSingleUserConfirmationEmail",
            Message = "No se logró armar la lista de destinatarios para enviar la confirmación de correo."
        };
        public static readonly Error EmailNotSentOnSingleUserConfirmationEmail = new()
        {
            Code = "Notification.SingleUserConfirmationEmail.EmailNotSentOnSingleUserConfirmationEmail",
            Message = "No se ha podido enviar el correo."
        };
        public static readonly Error CommunityNotfoundOnSingleUserConfirmationEmail = new()
        {
            Code = "Notification.SingleUserConfirmationEmail.CommunityNotfoundOnSingleUserConfirmationEmail",
            Message = "No se encontró la comunidad."
        };


        // Errores en SingleUserConfirmationEmail
        public static readonly Error TemplateNotFoundOnMassUserConfirmationEmail = new()
        {
            Code = "Notification.MassUserConfirmationEmail.TemplateNotFoundOnMassUserConfirmationEmail",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnMassUserConfirmationEmail = new()
        {
            Code = "Notification.MassUserConfirmationEmail.TemplateIdIsNullOnMassUserConfirmationEmail",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };
        public static readonly Error TemplateNotCreatedOnMassUserConfirmationEmail = new()
        {
            Code = "Notification.MassUserConfirmationEmail.TemplateNotCreatedOnMassUserConfirmationEmail",
            Message = "No se logró armar el template para enviar la confirmación de correo."
        };
        public static readonly Error RecipientsNotCreatedOnMassUserConfirmationEmail = new()
        {
            Code = "Notification.MassUserConfirmationEmail.RecipientsNotCreatedOnMassUserConfirmationEmail",
            Message = "No se logró armar la lista de destinatarios para enviar la confirmación de correo."
        };
        public static readonly Error EmailNotSentOnMassUserConfirmationEmail = new()
        {
            Code = "Notification.MassUserConfirmationEmail.EmailNotSentOnMassUserConfirmationEmail",
            Message = "No se ha podido enviar el correo."
        };
        public static readonly Error CommunityNotfoundOnMassUserConfirmationEmail = new()
        {
            Code = "Notification.MassUserConfirmationEmail.CommunityNotfoundOnMassUserConfirmationEmail",
            Message = "No se encontró la comunidad."
        };
        public static readonly Error CommunityNotfoundOnConfirmReservationEmail = new()
        {
            Code = "Notification.ConfirmReservationEmail.CommunityNotfoundOnConfirmReservationEmail",
            Message = "No se encontró la comunidad."
        };



        //Errores de MaintenanceCancellation
        public static readonly Error CommonSpaceNotFoundOnMaintenanceCancellation = new()
        {
            Code = "Notification.MaintenanceCancellation.CommonSpaceNotFoundOnMaintenanceCancellation",
            Message = "No se ha podido encontrar el espacio común."
        };
        public static readonly Error CommunityNotFoundOnMaintenanceCancellation = new()
        {
            Code = "Notification.MaintenanceCancellation.CommunityNotFoundOnMaintenanceCancellation",
            Message = "No se ha podido completar el template para enviar correo."
        };
        public static readonly Error TemplateNotFoundOnMaintenanceCancellation = new()
        {
            Code = "Notification.MaintenanceCancellation.TemplateNotFoundOnMaintenanceCancellation",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnMaintenanceCancellation = new()
        {
            Code = "Notification.MaintenanceCancellation.TemplateIdIsNullOnMaintenanceCancellation",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };
        public static readonly Error TemplateNotCreatedOnMaintenanceCancellation = new()
        {
            Code = "Notification.MaintenanceCancellation.TemplateNotCreatedOnMaintenanceCancellation",
            Message = "El templates no puede ser null para enviar el termino de mantención."
        };
        public static readonly Error EmailNotSentOnMaintenanceCancellation = new()
        {
            Code = "Notification.MaintenanceCancellation.EmailNotSentOnMaintenanceCancellation",
            Message = "No se ha podido enviar el correo."
        };


        //Errores de SimpleMessage

        public static readonly Error TemplateNotFoundOnSimpleMessage = new()
        {
            Code = "Notification.SimpleMessage.TemplateNotFoundOnSimpleMessage",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnSimpleMessage = new()
        {
            Code = "Notification.SimpleMessage.TemplateIdIsNullOnSimpleMessage",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };
        public static readonly Error RecipientsIsNullOnSimpleMessage = new()
        {
            Code = "Notification.SimpleMessage.RecipientsIsNullOnSimpleMessage",
            Message = "No se encontraron destinatarios"
        };

        //Errores de SingleUserSignUpSummary
        public static readonly Error EmailNotSentOnSingleUserSignUpSummary = new()
        {
            Code = "Notification.SingleUserSignUpSummary.EmailNotSentOnSingleUserSignUpSummary",
            Message = "No se ha podido enviar el correo."
        };
        public static readonly Error AdminUserNotExistOnSingleUserSignUpSummary = new()
        {
            Code = "Notification.SingleUserSignUpSummary.AdminUserNotExistOnSingleUserSignUpSummary",
            Message = "No se logró ubicar al administrador informado."
        };
        public static readonly Error CommunityNotFoundOnSingleUserSignUpSummary = new()
        {
            Code = "Notification.SingleUserSignUpSummary.CommunityNotFoundOnSingleUserSignUpSummary",
            Message = "No existe la comunidad consultada."
        };
        public static readonly Error TemplateNotFoundOnSingleUserSignUpSummary = new()
        {
            Code = "Notification.SingleUserSignUpSummary.TemplateNotFoundOnSingleUserSignUpSummary",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnSingleUserSignUpSummary = new()
        {
            Code = "Notification.SingleUserSignUpSummary.TemplateIdIsNullOnSingleUserSignUpSummary",
        };

        //Errores de MassUserSignUpSummary
        public static readonly Error EmailNotSentOnMassUserSignUpSummary = new()
        {
            Code = "Notification.MassUserSignUpSummary.EmailNotSentOnMassUserSignUpSummary",
            Message = "No se ha podido enviar el correo."
        };
        public static readonly Error AdminUserNotExistOnMassUserSignUpSummary = new()
        {
            Code = "Notification.MassUserSignUpSummary.AdminUserNotExistOnMassUserSignUpSummary",
            Message = "No se logró ubicar al administrador informado."
        };
        public static readonly Error CommunityNotFoundOnMassUserSignUpSummary = new()
        {
            Code = "Notification.MassUserSignUpSummary.CommunityNotFoundOnMassUserSignUpSummary",
            Message = "No existe la comunidad consultada."
        };
        public static readonly Error TemplateNotFoundOnMassUserSignUpSummary = new()
        {
            Code = "Notification.MassUserSignUpSummary.TemplateNotFoundOnMassUserSignUpSummary",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnMassUserSignUpSummary = new()
        {
            Code = "Notification.MassUserSignUpSummary.TemplateIdIsNullOnMassUserSignUpSummary",
        };



        //Errores de CommonExpenses
        public static readonly Error TemplateNotCreated = new()
        {
            Code = "Notification.CommonExpenses.TemplateNotCreated",
            Message = "El templates no puede ser null para enviar las notificaciones de Gastos común"
        };
        public static readonly Error TemplateNotFoundOnCommonExpenses = new()
        {
            Code = "Notification.CommonExpenses.TemplateNotFoundOnCommonExpenses",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnCommonExpenses = new()
        {
            Code = "Notification.CommonExpenses.TemplateIdIsNullOnCommonExpenses",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };

        // Errores de ReservationConfirmation
        public static readonly Error ReservationNotFound = new()
        {
            Code = "Notification.ReservationConfirmation.ReservationNotFound",
            Message = "No se logró ubicar la reserva consultada."
        };
        public static readonly Error TemplateNotFoundOnReservationConfirmation = new()
        {
            Code = "Notification.ReservationConfirmation.TemplateNotFoundOnCommonExpenses",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static readonly Error TemplateIdIsNullOnReservationConfirmation = new()
        {
            Code = "Notification.ReservationConfirmation.TemplateIdIsNullOnCommonExpenses",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };
        public static readonly Error TemplateNotCreatedOnReservationConfirmation = new()
        {
            Code = "Notification.ReservationConfirmation.TemplateNotCreatedOnReservationConfirmation",
            Message = "El templates no puede ser null para enviar confirmación de reserva."
        };
        public static readonly Error EmailNotSentOnReservationConfirmation = new()
        {
            Code = "Notification.ReservationConfirmation.EmailNotSentOnReservationConfirmation",
            Message = "No se pudo enviar el correo de confirmación de reserva"
        };

        // Errores de FineNotification
        public static readonly Error AssignatedFineNotFoundOnFineNotification = new(
            "Notification.FineNotification.AssignatedFineNotFoundOnFineNotification",
            "La multa asociada no existe en el sistema.");

        public static readonly Error TemplateNotFoundOnFineNotification = new(
            "Notification.FineNotification.TemplateNotFoundOnFineNotification",
            "No se ha podido encontrar el Dynamic template.");

        public static readonly Error TemplateIdIsNullOnFineNotification = new(
            "Notification.FineNotification.TemplateIdIsNullOnFineNotification",
            "El TempleID es nulo y se necesita para crear un Dynamic Email.");

        public static readonly Error EmailNotsendOnFineNotification = new(
            "Notification.FineNotification.EmailNotsendOnFineNotification",
            "No se logró enviar la notificacíón de Multa");

    }
}
