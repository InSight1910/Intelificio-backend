using Backend.Common.Response;
using Backend.Features.Attendees.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Attendees.Queries.GetAttendeeByReservation;

public class GetAttendeeByReservationQueryHandler(IntelificioDbContext context)
    : IRequestHandler<GetAttendeeByReservationQuery, Result>
{
    public async Task<Result> Handle(GetAttendeeByReservationQuery request, CancellationToken cancellationToken)
    {
        var reservationExist = await context.Reservations.AnyAsync(x => x.ID == request.ReservationId);
        if (!reservationExist) return Result.Failure(AttendeesErrors.ReservationNotFoundOnQuery);

        var response = await context.Attendees.Where(x => x.ReservationId == request.ReservationId).Select(x =>
            new GetAttendeeByReservationQueryResponse
            {
                Id = x.ID,
                Email = x.Email,
                Name = x.Name,
                Rut = x.Rut
            }).ToListAsync();

        return Result.WithResponse(new ResponseData() { Data = response });
    }
}