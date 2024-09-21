﻿using Backend.Common.Response;
using Backend.Features.Buildings.Commands.AddUnit;
using Backend.Features.Buildings.Commands.Create;
using Backend.Features.Buildings.Commands.Delete;
using Backend.Features.Buildings.Commands.RemoveUnit;
using Backend.Features.Buildings.Commands.Update;
using Backend.Features.Buildings.Queries.GetAllByCommunity;
using Backend.Features.Buildings.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Buildings.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController(IMediator mediator) : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateBuildingCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Created(),
                onFailure: BadRequest);
        }

        [HttpDelete("{iD}")]
        public async Task<IActionResult> Delete(int iD, [FromRoute] DeleteBuildingCommand command)
        {
            command.Id = iD;
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpPut("{iD}")]
        public async Task<IActionResult> Update(int iD, [FromBody] UpdateBuildingCommand command)
        {
            command.Id = iD;
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpPut("{BuildingId}/AddUnit/{UnitId}")]
        public async Task<IActionResult> Add(int BuildingId, int UnitId, [FromBody] AddUnitBuildingCommand command)
        {
            command.BuildingId = BuildingId;
            command.UnitId = UnitId;
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpPut("{BuildingId}/RemoveUnit/{UnitId}")]
        public async Task<IActionResult> Remove(int BuildingId, int UnitId, [FromBody] RemoveUnitBuildingCommand command)
        {
            command.BuildingId = BuildingId;
            command.UnitId = UnitId;
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpGet("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var query = new GetByIDQuery { BuildingId = ID };
            var building = await mediator.Send(query);
            return building.Match<IActionResult>(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

        [HttpGet("GetAllByCommunity/{ID}")]
        public async Task<IActionResult> GetAllByCommunity(int ID)
        {
            var query = new GetAllByCommunityQuery { CommunityId = ID };
            var buildings = await mediator.Send(query);
            return buildings.Match<IActionResult>(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

    }
}