using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.AssignedFines.Commands.Delete
{
    public class DeleteAssignedFinesCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int AssignedfineId { get; set; }
    }
}
