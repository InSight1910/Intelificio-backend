using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.AssignedFines.Commands.Update
{
    public class UpdateAssignedFinesCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int AssignedFineId { get; set; }
        public int FineId { get; set; }
        public int UnitId { get; set; }
        public DateTime EventDate { get; set; }
        public required string Comment { get; set; }
    }
}
