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
}