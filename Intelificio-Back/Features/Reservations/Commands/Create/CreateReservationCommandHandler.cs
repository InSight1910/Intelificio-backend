using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Reservations.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Reservations.Commands.Create;

public class CreateReservationCommandHandler(IntelificioDbContext context, IMapper _mapper)
    : IRequestHandler<CreateReservationCommand, Result>
{
    public async Task<Result> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var userExist = await context.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken);
        if (!userExist) return Result.Failure(ReservationErrors.UserNotFoundOnCreate);

        var spaceExist = await context.CommonSpaces.AnyAsync(x => x.ID == request.CommonSpaceId, cancellationToken);
        if (!spaceExist) return Result.Failure(ReservationErrors.CommonSpaceNotFoundOnCreate);

        var checkReservationExist = await context.Reservations.AnyAsync(
            x =>
                x.UserId == request.UserId &&
                x.Date.Date == request.Date.Date &&
                x.SpaceId == request.CommonSpaceId &&
                x.StartTime >= request.StartTime &&
                x.EndTime <= request.EndTime
            , cancellationToken);
        if (checkReservationExist) return Result.Failure(ReservationErrors.AlreadyExistOnCreate);
        var reservation = new Reservation
        {
            Date = request.Date,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = ReservationStatus.PENDING,
            UserId = request.UserId,
            SpaceId = request.CommonSpaceId
        };

        var result = await context.Reservations.AddAsync(reservation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<CreateReservationCommandResponse>(reservation);

        return Result.WithResponse(new ResponseData { Data = response });
    }
}