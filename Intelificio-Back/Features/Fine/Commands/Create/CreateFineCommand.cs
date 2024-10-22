using Backend.Common.Response;
using Backend.Models.Enums;
using MediatR;

namespace Backend.Features.Fine.Commands.Create
{
    public class CreateFineCommand : IRequest<Result>
    {
        public required string Name { get; set; }
        public required decimal Amount { get; set; }
        public required FineDenomination Status { get; set; }
        public required int CommunityId { get; set; }

    }
}
