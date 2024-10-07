using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Contact.Commands.Delete
{
    public class DeleteContactCommand: IRequest<Result>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
