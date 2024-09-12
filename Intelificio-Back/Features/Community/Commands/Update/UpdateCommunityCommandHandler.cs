using Backend.Common.Response;
using Backend.Models;
using MediatR;

namespace Backend.Features.Community.Commands.Update
{
    public class UpdateCommunityCommandHandler : IRequestHandler<UpdateCommunityCommand, Result>
    {
        private readonly IntelificioDbContext _context;

        public Task<Result> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
