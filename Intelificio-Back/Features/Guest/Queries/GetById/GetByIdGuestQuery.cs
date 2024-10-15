using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Guest.Queries.GetById
{
    public class GetByIdGuestQuery : IRequest<Result>
    {
        [JsonIgnore]
        public required int Id { get; set; }
    }
}
