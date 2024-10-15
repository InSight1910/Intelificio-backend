using System.Security.Cryptography;
using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Notification.Commands.Reservation.ReservationConfirmation;
using Backend.Features.Reservations.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Security.Cryptography.RandomNumberGenerator;

namespace Backend.Features.Reservations.Commands.Create;

public class CreateReservationCommandHandler(IntelificioDbContext context, IMapper _mapper, IMediator mediator)
    : IRequestHandler<CreateReservationCommand, Result>
{
    public async Task<Result> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var userExist = await context.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken);
        if (!userExist) return Result.Failure(ReservationErrors.UserNotFoundOnCreate);

        var spaceExist = await context.CommonSpaces.AnyAsync(x => x.ID == request.CommonSpaceId, cancellationToken);
        if (!spaceExist) return Result.Failure(ReservationErrors.CommonSpaceNotFoundOnCreate);
        TimeSpan.TryParse(request.StartTime, out var startTime);
        TimeSpan.TryParse(request.EndTime, out var endTime);
        var checkReservationExist = await context.Reservations.AnyAsync(
            x =>
                x.UserId == request.UserId &&
                x.Date.Date == request.Date.Date &&
                x.SpaceId == request.CommonSpaceId &&
                x.StartTime >= startTime &&
                x.EndTime <= endTime
            , cancellationToken);
        if (checkReservationExist) return Result.Failure(ReservationErrors.AlreadyExistOnCreate);
        var reservation = new Reservation
        {
            Date = request.Date,
            StartTime = startTime,
            EndTime = endTime,
            Status = ReservationStatus.PENDING,
            UserId = request.UserId,
            SpaceId = request.CommonSpaceId,
            ConfirmationToken = CreateRefreshToken(),
            ExpirationDate = DateTime.UtcNow.AddMinutes(10)
        };

        var result = await context.Reservations.AddAsync(reservation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<CreateReservationCommandResponse>(reservation);

        var confirmReservationEmailCommand = new ConfirmReservationEmailCommand { ReservationID = response.Id };
        _ = await mediator.Send(confirmReservationEmailCommand);
    

        return Result.WithResponse(new ResponseData { Data = response });
    }

    private string CreateRefreshToken()
    {
        var randomNumber = new byte[64];

        using (var numberGenerator = RandomNumberGenerator.Create())
        {
            numberGenerator.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }
}