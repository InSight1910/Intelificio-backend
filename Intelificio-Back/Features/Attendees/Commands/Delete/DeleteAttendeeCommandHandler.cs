using Backend.Common.Response;
using Backend.Features.Attendees.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Attendees.Commands.Delete;

public class DeleteAttendeeCommandHandler(IntelificioDbContext context) : IRequestHandler<DeleteAttendeeCommand, Result>
{
    public async Task<Result> Handle(DeleteAttendeeCommand request, CancellationToken cancellationToken)
    {
        var attendee = await context.Attendees.FirstOrDefaultAsync(x => x.ID == request.AttendeeId);
        if (attendee is null) return Result.Failure(AttendeesErrors.AttendeeNotFoundOnDelete);
        context.Attendees.Remove(attendee);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}