using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Contact.Commands.Create
{
    public class CreateContactCommand : IRequest<Result>
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Service { get; set; }
        public required int CommunityId { get; set; }
    }
}
