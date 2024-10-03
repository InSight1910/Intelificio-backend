using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Guest.Commands.Delete
{
    public class DeleteGuestCommand : IRequest<Result>
    {
        public required int GuestId { get; set; }
    }
}
