using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Reservations.Commands;

public class ConfirmReservationCommand : IRequest<Result>
{
    public int ReservationId { get; set; }
    public string token { get; set; }
}