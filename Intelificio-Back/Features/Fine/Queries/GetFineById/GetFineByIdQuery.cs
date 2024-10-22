using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Fine.Queries.GetFineById
{
    public class GetFineByIdQuery : IRequest<Result>
    {
        [JsonIgnore]
        public int FineId { get; set; }
    }
}
