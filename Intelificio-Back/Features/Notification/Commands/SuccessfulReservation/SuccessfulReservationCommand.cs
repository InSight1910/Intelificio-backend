using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.SuccessfulReservation
{
    public class SuccessfulReservationCommand: IRequest<Result>
    {
        public required int ReservationID { get; set; }
    }
}
