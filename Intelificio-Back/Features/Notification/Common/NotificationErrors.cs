﻿using Backend.Common.Response;

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

        //Errores de CommonExpenses
        public static Error TemplateNotCreated = new()
        {
            Code = "Notification.CommonExpenses.TemplateNotCreated",
            Message = "El templates no puede ser null para enviar las notificaciones de Gastos común"
        };

    }
}