using Backend.Common.Response;
using Backend.Features.Reservations.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Backend.Features.Reservations.Commands;

public class ConfirmReservationCommandHandler(IntelificioDbContext context)
    : IRequestHandler<ConfirmReservationCommand, Result>
{
    public async Task<Result> Handle(ConfirmReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await context.Reservations.FirstOrDefaultAsync(x => x.ID == request.ReservationId);
        if (reservation is null) return Result.Failure(ReservationErrors.ReservationNotFoundOnConfirm);

        if (reservation.ConfirmationToken != request.token) return Result.Failure(ReservationErrors.TokenNotFoundOnReservationOnConfirmReservation);
        reservation.ConfirmationToken = CreateNewRefreshToken();
        reservation.ExpirationDate = DateTime.UtcNow.AddHours(24);

        var utc = DateTime.UtcNow.AddMinutes(10);

        if (reservation.ExpirationDate < DateTime.UtcNow.AddMinutes(10))
        {
            reservation.Status = ReservationStatus.CANCELLED;
            await context.SaveChangesAsync(cancellationToken);

            return Result.Failure(ReservationErrors.ExpiredTokenOnConfirmReservation);
        }

        reservation.Status = ReservationStatus.CONFIRMED;
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private string CreateNewRefreshToken()
    {
        var randomNumber = new byte[64];

        using (var numberGenerator = RandomNumberGenerator.Create())
        {
            numberGenerator.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }
}