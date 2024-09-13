using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Community.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Commands.Update
{
    public class UpdateCommunityCommandHandler : IRequestHandler<UpdateCommunityCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<UpdateCommunityCommandHandler> _logger;
        private readonly IMapper _mapper;
        public UpdateCommunityCommandHandler(IntelificioDbContext context, ILogger<UpdateCommunityCommandHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<Result> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
        {
            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.Id);
            if (community == null) return Result.Failure(CommunityErrors.CommunityNotFoundDelete);

            community = _mapper.Map(request, community);

            _ = _context.Community.Update(community);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
