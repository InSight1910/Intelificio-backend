using Backend.Common.Response;
using Backend.Features.AssignedFines.Commands.Create;
using Backend.Features.AssignedFines.Commands.Delete;
using Backend.Features.AssignedFines.Commands.Update;
using Backend.Features.AssignedFines.Queries.GetAllAssignedFinesByCommunity;
using Backend.Features.AssignedFines.Queries.GetAssignedFinesById;
using Backend.Features.AssignedFines.Queries.GetAssignedFinesByUnitId;
using Backend.Features.AssignedFines.Queries.GetAssignedFinesByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.AssignedFines.Common
{
    [Route("api/[controller]")]
    public class AssignedFinesController(IMediator mediator) : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> CreateAsignedFine([FromBody] CreateAssignedFinesCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAssignedFine(int Id, [FromRoute] DeleteAssignedFinesCommand command)
        {
            command.AssignedfineId = Id;
            var result = await mediator.Send(command);
            return result.Match(
            onSuccess: (_) => Ok(),
            onFailure: BadRequest);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAssignedFine(int Id, [FromBody] UpdateAssignedFinesCommand command)
        {
            command.AssignedFineId = Id;
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAssignedFinesById(int Id)
        {
            var assignedFine = await mediator.Send(new GetAssignedFinesByIdQuery { AssignedFineId = Id });
            return assignedFine.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }

        [HttpGet("GetByUnit/{Id}")]
        public async Task<IActionResult> GetAssignedFinesByUnitId(int Id)
        {
            var assignedFine = await mediator.Send(new GetAssignedFinesByUnitIdQuery { UnitId = Id });
            return assignedFine.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }

        [HttpGet("GetByUser/{Id}")]
        public async Task<IActionResult> GetAssignedFinesByUserId(int Id)
        {
            var assignedFine = await mediator.Send(new GetAssignedFinesByUserIdQuery { UserId = Id });
            return assignedFine.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }

        [HttpGet("GetByCommunity/{Id}")]
        public async Task<IActionResult> GetAllAssignedFinesByCommunityId(int Id)
        {
            var assignedFines = await mediator.Send(new GetAllAssignedFinesByCommunityIdQuery { CommunityId = Id });
            return assignedFines.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }


    }
}
