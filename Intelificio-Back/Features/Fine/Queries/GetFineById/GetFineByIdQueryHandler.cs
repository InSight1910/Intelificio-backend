using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Fine.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Fine.Queries.GetFineById
{
    public class GetFineByIdQueryHandler(IntelificioDbContext conext, ILogger<GetFineByIdQueryHandler> logger, IMapper mapper): IRequestHandler<GetFineByIdQuery, Result>
    {
        private readonly IntelificioDbContext _context = conext;
        private readonly ILogger<GetFineByIdQueryHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(GetFineByIdQuery request, CancellationToken cancellationToken)
        {
            var fine = await _context.Fine.FirstOrDefaultAsync(x => x.ID == request.FineId, cancellationToken);
            if (fine is null) return Result.Failure(FineErrors.FineNotFoundOnGetFineById);

            var response = _mapper.Map<GetFineByIdQueryResponse>(fine);

            return Result.WithResponse(new ResponseData()
            {
                Data = response
            });

        }

    }
}
