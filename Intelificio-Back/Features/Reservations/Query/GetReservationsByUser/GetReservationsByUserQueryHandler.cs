using Backend.Common.Response;
using Backend.Features.Reservations.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Reservations.Query.GetReservationsByUser;

public class GetReservationsByUserQueryHandler(IntelificioDbContext context)
    : IRequestHandler<GetReservationsByUserQuery, Result>
{
    public async Task<Result> Handle(GetReservationsByUserQuery request, CancellationToken cancellationToken)
    {
        var userExist = await context.Users.AnyAsync(x => x.Id == request.UserId);
        if (!userExist) return Result.Failure(ReservationErrors.UserNotFoundOnQuery);

        var result = await context.Reservations
            .Include(x => x.Spaces)
            .Include(x => x.Attendees)
            .Where(x => x.UserId == request.UserId)
            .Select(x => new GetReservationsByUserQueryResponse
            {
                Status = (int)x.Status,
                StartTime = TimeOnly.FromTimeSpan(x.StartTime).ToString(@"hh\:mm tt"),
                EndTime = TimeOnly.FromTimeSpan(x.EndTime).ToString(@"hh\:mm tt"),
                SpaceName = x.Spaces.Name,
                Date = x.Date,
                Id = x.ID,
                Attendees = x.Attendees.Count(),
                Location = x.Spaces.Location
            })
            .OrderByDescending(x => x.Status)
            .ToListAsync();

        if (!result.Any()) return Result.Failure(ReservationErrors.ReservationsNotFoundOnQuery);
        return Result.WithResponse(new ResponseData()
        {
            Data = result
        });
    }
}