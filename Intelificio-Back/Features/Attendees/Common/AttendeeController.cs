using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Attendees.Common;

[ApiController]
[Route("api/[controller]")]
public class AttendeeController(IMediator mediator) : ControllerBase
{
}