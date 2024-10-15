using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Attendees.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Attendees.Commands.Create;

public class CreateAttendeeCommandHandler(IntelificioDbContext context, IMapper mapper)
    : IRequestHandler<CreateAttendeeCommand, Result>
{
    public async Task<Result> Handle(CreateAttendeeCommand request, CancellationToken cancellationToken)
    {
        var reservation = await context.Reservations
            .Include(x => x.Spaces)
            .Include(x => x.Attendees)
            .Where(x => x.ID == request.ReservationId)
            .Select(x =>
                new
                {
                    x.Spaces.Capacity,
                    x.Attendees.Count
                })
            .FirstOrDefaultAsync();
        if (reservation is null) return Result.Failure(AttendeesErrors.ReservationNotFoundOnCreate);

        if (reservation.Count >= reservation.Capacity) return Result.Failure(AttendeesErrors.CapacityExceeded);

        var attendeeExist =
            await context.Attendees.AnyAsync(x => x.ReservationId == request.ReservationId && x.Rut == request.RUT);
        if (attendeeExist) return Result.Failure(AttendeesErrors.AttendeeAlreadyExist);

        var attendee = mapper.Map<Attendee>(request);
        var result = await context.Attendees.AddAsync(attendee, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        request.Id = result.Entity.ID;

        return Result.WithResponse(new ResponseData
        {
            Data = request
        });
    }
}