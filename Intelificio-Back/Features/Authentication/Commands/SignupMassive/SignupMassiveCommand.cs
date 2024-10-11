using Backend.Common.Response;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.Features.Authentication.Commands.SignupMassive
{
    public class SignupMassiveCommand : IRequest<Result>
    {
        public required IFormFile File { get; set; }
        [BindNever]
        public MemoryStream? Stream { get; set; }
        public int CreatorID { get; set; }
        public int CommunityID { get; set; }

        public bool IsMassive { get; set; } = true;


    }
}
