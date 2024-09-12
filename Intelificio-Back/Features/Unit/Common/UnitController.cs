using Backend.Common.Response;
using Backend.Features.Unit.Queries.GetByID;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Unit.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await mediator.Send(new GetByIDQuery { UnitId = id });
            return result.Match<IActionResult>(resultado => Ok(resultado), resultado => NotFound(resultado));
        }
    }
}
