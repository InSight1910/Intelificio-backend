using Backend.Common.Response;
using Backend.Features.Reservations.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Reservations.Commands;

public class ConfirmReservationCommandHandler(IntelificioDbContext context)
    : IRequestHandler<ConfirmReservationCommand, Result>
{
    public async Task<Result> Handle(ConfirmReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await context.Reservations.FirstOrDefaultAsync(x => x.ID == request.ReservationId);
        if (reservation is null) return Result.Failure(ReservationErrors.ReservationNotFoundOnConfirm);

        if (reservation.ConfirmationToken != request.token)
            return Result.Failure(ReservationErrors.ConfirmationTokenNotCorrect);
        reservation.ConfirmationToken = null;
        reservation.ExpirationDate = DateTime.MinValue;

        if (reservation.ExpirationDate < DateTime.UtcNow.AddMinutes(10))
        {
            reservation.Status = ReservationStatus.CANCELLED;
            await context.SaveChangesAsync(cancellationToken);

            return Result.Failure(null);
        }

        reservation.Status = ReservationStatus.CONFIRMED;
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}