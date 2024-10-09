using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Maintenance.Queries.GetById
{
    public class GetByIdQuery : IRequest<Result>
    {
        [JsonIgnore]
        public required int MaintenanceId { get; set; }
    }
}
