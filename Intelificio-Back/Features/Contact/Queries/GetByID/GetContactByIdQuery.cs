using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Contact.Queries.GetByID
{
    public class GetContactByIdQuery: IRequest<Result>
    {
        [JsonIgnore]
        public required int Id { get; set; }
    }
}
