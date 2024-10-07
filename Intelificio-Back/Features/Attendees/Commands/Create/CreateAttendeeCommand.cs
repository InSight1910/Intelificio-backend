using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Attendees.Commands.Create;

public class CreateAttendeeCommand : IRequest<Result>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string RUT { get; set; }
    public int ReservationId { get; set; }
}