using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Reservations.Commands.Create;

public class CreateReservationCommand : IRequest<Result>
{
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int CommonSpaceId { get; set; }
}