using Backend.Common.Response;
using Backend.Features.Guest.Commands.Create;
using Backend.Features.Guest.Commands.Delete;
using Backend.Features.Guest.Commands.Update;
using Backend.Features.Guest.Queries.GetById;
using Backend.Features.Guest.Queries.GetAllByUnit;
using Backend.Features.Guest.Queries.GetAllByEntryTime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quartz.Xml.JobSchedulingData20;
using System;

namespace Backend.Features.Guest.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController(IMediator mediator) : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateGuestCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Created(),
                onFailure: BadRequest);
        }

        [HttpDelete("{iD}")]
        public async Task<IActionResult> Delete(int iD, [FromRoute] DeleteGuestCommand command)
        {
            command.GuestId = iD;
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpPut("{iD}")]
        public async Task<IActionResult> Update(int iD, [FromBody] UpdateGuestCommand command)
        {
            command.GuestId = iD;
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await mediator.Send(new GetByIdGuestQuery { GuestId = id });
            return result.Match<IActionResult>(resultado => Ok(resultado), resultado => NotFound(resultado));
        }

        [HttpGet("GetAllByUnit/{unitId}")]
        public async Task<IActionResult> GetAllByUnit(int unitId)
        {
            var result = await mediator.Send(new GetAllByUnitGuestQuery { UnitId = unitId });
            return result.Match<IActionResult>(resultado => Ok(resultado), resultado => NotFound(resultado));
        }

        [HttpGet("GetAllByEntryTime")]
        public async Task<IActionResult> GetAllByEntryTime([FromQuery] DateTime entryTime)
        {
            var result = await mediator.Send(new GetAllByEntryTimeGuestQuery { EntryTime = entryTime});
            return result.Match<IActionResult>(resultado => Ok(resultado), resultado => NotFound(resultado));
        }
    }
}
