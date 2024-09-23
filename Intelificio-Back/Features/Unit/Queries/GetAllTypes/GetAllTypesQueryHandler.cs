using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Queries.GetAllTypes
{
    public class GetAllTypesQueryHandler: IRequestHandler<GetAllTypesQuery, Result>
    {
        private readonly IntelificioDbContext _context;

        public GetAllTypesQueryHandler(IntelificioDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _context.UnitTypes.Select(x => new GetAllTypesQueryResponse { Id = x.ID, Name = x.Description }).ToListAsync();
            return Result.WithResponse(new ResponseData { Data = types});
        }
    }
}
