using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Attendees.Commands.Delete;

public class DeleteAttendeeCommand : IRequest<Result>
{
    public required int AttendeeId { get; set; }
}