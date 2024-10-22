using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Fine.Commands.Delete
{
    public class DeleteFineCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int FineId { get; set; }
    }
}
