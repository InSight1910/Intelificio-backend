using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Community.Queries.GetAllByUser;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Queries.GetByID
{
    public class GetByIDQueryHandler : IRequestHandler<GetByIDQuery, Result>
    {

        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetByIDQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetByIDQueryHandler(IntelificioDbContext context, ILogger<GetByIDQueryHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetByIDQuery request, CancellationToken cancellationToken)
        {
            var unit = _context.Units
                              .Where(x => x.ID == request.UnitId) 
                              .Include(x => x.Type)
                              .Select(x => new GetByIDResponse { 
                                    Number = x.Number,
                                    UnitType = x.Type.Description
                              })
                              .FirstOrDefault();

            if (unit == null) return Result.Failure(UnitErrors.UnitNotFound);

            return Result.WithResponse(new ResponseData()
            {
                Data = unit
            });
        }
    }
}
