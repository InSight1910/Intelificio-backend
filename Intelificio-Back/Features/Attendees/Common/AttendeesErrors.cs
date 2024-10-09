using Backend.Common.Response;

namespace Backend.Features.Attendees.Common;

public class AttendeesErrors
{
    public static Error ReservationNotFoundOnCreate = new()
        { Code = "Attendee.Create.ReservationNotFoundOnCreate", Message = "La reservacion indicada no existe." };

    public static Error AttendeeAlreadyExist = new()
    {
        Code = "Attendee.Create.AttendeeAlreadyExist",
        Message = "El invitado ya se encuentra registrado."
    };

    public static Error AttendeeNotFoundOnDelete = new()
    {
        Code = "Attendee.Create.AttendeeNotFoundOnDelete",
        Message = "El invitado no existe."
    };

    public static Error ReservationNotFoundOnQuery = new()
    {
        Code = "Attendee.GetAttendeeByReservation.ReservationNotFoundOnQuery",
        Message = "No se ha encontrado la reservacion."
    };

    public static Error AttendeesNotFoundOnQuery = new()
    {
        Code = "Attendee.GetAttendeeByReservation.AttendeesNotFoundOnQuery",
        Message = "No existen invitados registrados para esta reserva."
    };

    public static Error CapacityExceeded = new()
    {
        Code = "Attendee.Create.CapacityExceeded",
        Message = "La cantidad de invitados no puede sobrepasar la capacidad del espacio."
    };
}