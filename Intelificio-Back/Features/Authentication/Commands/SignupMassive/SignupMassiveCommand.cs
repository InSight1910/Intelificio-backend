using Backend.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.Features.Authentication.Commands.SignupMassive
{
    public class SignupMassiveCommand : IRequest<Result>
    {
        public required IFormFile File { get; set; }
        [BindNever]
        public MemoryStream? Stream { get; set; }
    }
}
