﻿using Backend.Common.Response;
using Backend.Features.Community.Commands.Assign;
using Backend.Features.Community.Commands.Create;
using Backend.Features.Community.Commands.Delete;
using Backend.Features.Community.Commands.RemoveUser;
using Backend.Features.Community.Commands.Update;
using Backend.Features.Community.Queries.GetAll;
using Backend.Features.Community.Queries.GetAllByUser;
using Backend.Features.Community.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Community.Common
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CommunityController(IMediator mediator) : ControllerBase
    {
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUser(int userId)
        {
            var query = new GetAllByUserQuery { UserId = userId };

            var result = await mediator.Send(query);
            return result.Match<IActionResult>(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllCommunitiesQuery { });
            return Ok(result.Response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetByIdCommunityQuery { Id = id });
            return result.Match<IActionResult>(
                onSuccess: response => Ok(response),
                onFailure: NotFound
                );
        }

        [HttpPut("add/{id}/{userId}")]
        public async Task<IActionResult> AddUserToCommunity(int id, int userId)
        {
            var result = await mediator.Send(new AddUserCommunityCommand { CommunityId = id, UserId = userId });
            return result.Match<IActionResult>(
                onSuccess: response => Ok(response),
                onFailure: NotFound
                );
        }

        [HttpPut("remove/{id}/{userId}")]
        public async Task<IActionResult> RemoveUserToCommunity(int id, int userId)
        {
            var result = await mediator.Send(new RemoveUserCommunityCommand { CommunityId = id, UserId = userId });
            return result.Match<IActionResult>(
                onSuccess: response => Ok(response),
                onFailure: NotFound
                );
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommunityCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: _ => Created(),
                onFailure: (result) => BadRequest(result)
                );
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await mediator.Send(new DeleteCommunityCommand { Id = id });
            return result.Match<IActionResult>(
                onSuccess: _ => Ok(),
                onFailure: BadRequest
                );
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommunityCommand command)
        {
            command.Id = id;
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: _ => Ok(),
                onFailure: BadRequest
                );
        }


    }
}
