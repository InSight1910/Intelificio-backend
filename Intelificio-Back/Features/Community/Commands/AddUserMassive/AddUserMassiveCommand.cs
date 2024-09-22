using Backend.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.Features.Community.Commands.AddUserMassive
{
    public class AddUserMassiveCommand : IRequest<Result>
    {
        public required IFormFile File { get; set; }
        [BindNever]
        public MemoryStream? Stream { get; set; }
    }
}
