using AutoMapper;
using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Attendees.Commands.Create;

public class CreateAttendeeCommandHandler(IntelificioDbContext context, IMapper mapper)
    : IRequestHandler<CreateAttendeeCommand, Result>
{
    public async Task<Result> Handle(CreateAttendeeCommand request, CancellationToken cancellationToken)
    {
        var reservationExist = await context.Reservations.AnyAsync(x => x.ID == request.ReservationId);
        if (reservationExist) return Result.Failure(null);

        var attendeeExist =
            await context.Attendees.AnyAsync(x => x.ReservationId == request.ReservationId && x.Rut == request.RUT);
        if (attendeeExist) return Result.Failure(null);

        var attendee = mapper.Map<Attendee>(request);
        var result = await context.Attendees.AddAsync(attendee, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);


        return Result.WithResponse(new ResponseData
        {
            Data = result.Entity
        });
    }
}