using AutoMapper;
using Backend.Common.Response;
using Backend.Models;
using MediatR;

namespace Backend.Features.CommonSpaces.Commands.Create
{
    public class CreateCommonSpaceCommandHandler : IRequestHandler<CreateCommonSpaceCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly IMapper _mapper;

        public CreateCommonSpaceCommandHandler(IntelificioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateCommonSpaceCommand request, CancellationToken cancellationToken)
        {
            var existCommunity = _context.Community.Any(x => x.ID == request.CommunityId);
            if (!existCommunity) return Result.Failure("Community not found");
            var existSpace = _context.CommonSpaces.Any(x => x.Name == request.Name);
            if (existSpace) return Result.Failure("Common space already exists");
            var space = _mapper.Map<CommonSpace>(request);

            var result = await _context.CommonSpaces.AddAsync(space);
            await _context.SaveChangesAsync(cancellationToken);
            var response = _mapper.Map<CreateCommonSpaceCommandResponse>(result.Entity);
            return Result.WithResponse(new ResponseData { Data = response });
        }
    }
}
