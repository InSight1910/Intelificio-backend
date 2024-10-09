using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Attendees.Queries.GetAttendeeByReservation;

public class GetAttendeeByReservationQuery : IRequest<Result>
{
    public required int ReservationId { get; set; }
}