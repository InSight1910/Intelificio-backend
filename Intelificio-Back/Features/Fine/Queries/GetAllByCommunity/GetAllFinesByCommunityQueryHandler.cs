using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Fine.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Fine.Queries.GetAllByCommunity
{
    public class GetAllFinesByCommunityQueryHandler(IntelificioDbContext context, ILogger<GetAllFinesByCommunityQueryHandler> logger, IMapper mapper) : IRequestHandler<GetAllFinesByCommunityQuery, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<GetAllFinesByCommunityQueryHandler> _logget = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(GetAllFinesByCommunityQuery request, CancellationToken cancellationToken)
        {

            var checkCommunity = await _context.Community.AnyAsync(x => x.ID == request.CommunityId, cancellationToken);
            if (!checkCommunity) return Result.Failure(FineErrors.CommunityNotFoundOnGetAllFinesByCommunityQuery);
            var fines = await _context.Fine
                .Where(c => c.CommunityId == request.CommunityId)
                .Select(f => new GetAllFinesByCommunityQueryResponse
                {
                    FineId = f.ID,
                    Name = f.Name,
                    Amount = f.Amount,
                    Status = f.Status,
                    CommunityId = f.CommunityId 
                }).ToListAsync(cancellationToken: cancellationToken);

            return Result.WithResponse(new ResponseData()
            {
                Data = fines
            });

        }

    }
}
