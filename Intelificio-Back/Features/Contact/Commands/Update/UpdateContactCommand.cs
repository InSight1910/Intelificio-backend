using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Contact.Commands.Update
{
    public class UpdateContactCommand : IRequest<Result>
    {
        [JsonIgnore]
        public  int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Service { get; set; }

    }
}
