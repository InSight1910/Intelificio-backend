using Backend.Common.Response;
using Backend.Features.Notification.Commands.Package;
using Backend.Features.Notification.Commands.SingleMessage;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Notification.Common
{
    [Route("api/[Controller]")]
    [ApiController]
    public class NotificationController(IMediator mediator) : ControllerBase
    {
        [HttpPost("SingleMessage")]
        public async Task<IActionResult> SendSingleEmail([FromBody] SingleMessageCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: _ => Created(),
                onFailure: (result) => BadRequest(result)
                );
        }

        [HttpPost("Package/{ID}")]
        public async Task<IActionResult> SendPackageNotification(int ID, [FromRoute] PackageCommand command)
        {
            command.PackageID = ID;
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: _ => Created(),
                onFailure: (result) => BadRequest(result)
                );
        }
    }
}
