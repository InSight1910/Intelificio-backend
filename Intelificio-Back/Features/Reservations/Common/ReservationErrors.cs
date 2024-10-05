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
}