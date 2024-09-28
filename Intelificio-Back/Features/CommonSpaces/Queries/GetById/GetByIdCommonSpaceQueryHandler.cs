using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.CommonSpaces.Queries.GetById
{
    public class GetByIdCommonSpaceQueryHandler : IRequestHandler<GetByIdCommonSpaceQuery, Result>
    {
        private readonly IntelificioDbContext _context;

        public GetByIdCommonSpaceQueryHandler(IntelificioDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetByIdCommonSpaceQuery request, CancellationToken cancellationToken)
        {
            var space = await _context.CommonSpaces
                .Where(x => x.ID == request.CommonSpaceId)
                .Select(x => new GetByIdCommonSpaceQueryResponse
                {
                    ID = x.ID,
                    Name = x.Name,
                    Capacity = x.Capacity,
                    Location = x.Location,
                    IsInMaintenance = x.IsInMaintenance
                })
                .FirstOrDefaultAsync();
            if (space == null) return Result.Failure("Common space not found");

            return Result.WithResponse(new ResponseData { Data = space });
        }
    }
}
