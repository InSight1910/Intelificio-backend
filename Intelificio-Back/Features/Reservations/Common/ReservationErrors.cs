using Backend.Common.Response;

namespace Backend.Features.Reservations.Common;

public class ReservationErrors
{
    public static Error UserNotFoundOnCreate = new()
    {
        Code = "Reservation.Create.UserNotFound",
        Message = "El usuario no se encuentra en nuestros registros."
    };

    public static Error CommonSpaceNotFoundOnCreate = new()
    {
        Code = "CommonSpace.CommonSpaceNotFound",
        Message = "El espacio no se encuentra en nuestros registros."
    };

    public static Error AlreadyExistOnCreate = new()
    {
        Code = "Reservation.ReservationAlreadyExist",
        Message = "Ya existe una reserva para este espacio en el rango seleccionado."
    };

    public static Error CommunityNotFoundOnGetByCommunityAndMonth = new()
    {
        Code = "Reservation.GetReservationsByCommunityAndMonth.CommunityNotFound",
        Message = "La comunidad enviada no existe."
    };

    public static Error ReservationNotFoundOnConfirm = new()
    {
        Code = "Reservation.ConfirmReservation.ReservationNotFound",
        Message = "La reserva no existe."
    };

    public static Error TokenNotValidOnConfirm = new()
    {
        Code = "Reservation.ConfirmReservation.TokenNotValidOnConfirm",
        Message = "El token no es valido."
    };

    public static Error TokenExpiredOnConfirm = new()
    {
        Code = "Reservation.ConfirmReservation.TokenExpiredOnConfirm",
        Message = "El token se encuentra expirado."
    };

    public static Error UserNotFoundOnQuery = new()
    {
        Code = "Reservation.GetReservationsByUser.UserNotFound",
        Message = "El usuario no se encuentra en nuestros registros."
    };

    public static Error ReservationsNotFoundOnQuery = new()
    {
        Code = "Reservation.GetReservationsByUser.ReservationsNotFound",
        Message = "No se encontraron registros a su nombre."
    };

    public static Error ReservationsNotFoundOnQueryByID = new()
    {
        Code = "Reservation.GetReservationById.ReservationsNotFoundOnQueryByID",
        Message = "No se encontró reserva consultada."
    };

    public static Error ExpiredTokenOnConfirmReservation = new()
    {
        Code = "Reservation.ConfirmReservation.ExpiredTokenOnConfirmReservation",
        Message = "Ha expirado el tiempo para confirmar la reserva."
    };
    public static Error TokenNotFoundOnReservationOnConfirmReservation = new()
    {
        Code = "Reservation.ConfirmReservation.TokenNotFoundOnReservationOnConfirmReservation",
        Message = "Su reserva ya se encuentra confirmada."
    };
}