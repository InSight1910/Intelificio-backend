using Backend.Common.Response;

namespace Backend.Features.Guest.Common
{
    public class GuestErrors
    {
        public static Error UnitNotFoundCreateGuest = new Error(
        "Guest.CreateGuest.UnitNotFoundCreateGuest", "La unidad no fue encontrada");

        public static Error GuestAlreadyExists = new Error(
        "Guest.CreateGuest.GuestAlreadyExists", "La visita ya ha sido registrada");

        //-----------------------------------------------------------------------------

        public static Error GuestNotFoundDelete = new Error(
        "Guest.Delete.GuestNotFoundDelete", "La visita no fue encontrada");

        //-----------------------------------------------------------------------------

        public static Error GuestNotFoundUpdate = new Error(
        "Guest.Update.GuestNotFoundUpdate", "La visita no fue encontrada");

        //-----------------------------------------------------------------------------

        public static Error GuestNotFoundGetById = new Error(
        "Guest.GetById.GuestNotFoundGetById", "La visita no fue encontrada");
    }

}