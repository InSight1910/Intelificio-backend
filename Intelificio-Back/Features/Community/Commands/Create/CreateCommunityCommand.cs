using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Community.Commands.Create
{
    public class CreateCommunityCommand : IRequest<Result>
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public int MunicipalityId { get; set; }
        public string RUT { get; internal set; }
    }
}
