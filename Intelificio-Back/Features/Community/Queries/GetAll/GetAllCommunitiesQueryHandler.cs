using AutoMapper;
using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Queries.GetAll
{
    public class GetAllCommunitiesQueryHandler : IRequestHandler<GetAllCommunitiesQuery, Result>
    {
        private readonly ILogger<GetAllCommunitiesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IntelificioDbContext _context;

        public GetAllCommunitiesQueryHandler(ILogger<GetAllCommunitiesQueryHandler> logger, IMapper mapper, IntelificioDbContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result> Handle(GetAllCommunitiesQuery request, CancellationToken cancellationToken)
        {
            var communities = await _context.Community.ToListAsync();
            var response = _mapper.Map<List<GetAllCommunitiesResponse>>(communities);
            return Result.WithResponse(new ResponseData
            {
                Data = response
            });

        }
    }
}
