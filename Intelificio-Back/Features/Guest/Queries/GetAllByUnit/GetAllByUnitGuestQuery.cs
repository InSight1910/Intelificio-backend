using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Guest.Queries.GetAllByUnit
{
    public class GetAllByUnitGuestQuery : IRequest<Result>
    {
        [JsonIgnore]
        public required int UnitId { get; set; }
    }
}
