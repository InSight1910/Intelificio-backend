using Backend.Common.Response;
using Backend.Features.Notification.Commands.Reservation.ReservationCancellation;
using Backend.Features.Reservations.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Reservations.Commands.CancelReservation
{
    public class CancelReservationCommandHandler(IntelificioDbContext context, IMediator mediator)
        : IRequestHandler<CancelReservationCommand, Result>
    {
        public async Task<Result> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await context.Reservations.FirstOrDefaultAsync(x => x.ID == request.ReservationId);
            if (reservation is null) return Result.Failure(ReservationErrors.ReservationNotFoundOnConfirm);

            if(reservation.Status != ReservationStatus.PENDING && 
                reservation.Status != ReservationStatus.CONFIRMED)
            return Result.Failure("La reserva ya se encuentra cancelada o no está en un estado válido para ser cancelada.");

            reservation.Status = ReservationStatus.CANCELLED;
            await context.SaveChangesAsync(cancellationToken);

            var reservationCancellationCommand = new ReservationCancellationCommand { ReservationID = reservation.ID };
            _ = await mediator.Send(reservationCancellationCommand);

            return Result.Success();
        }

    }
}
