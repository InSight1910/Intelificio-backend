using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Fine.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Fine.Commands.Update
{
    public class UpdateFineHandler(IntelificioDbContext context, ILogger<UpdateFineHandler> logger, IMapper mapper): IRequestHandler<UpdateFineCommand, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<UpdateFineHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(UpdateFineCommand request, CancellationToken cancellationToken) 
        {
            var fine = await _context.Fine.FirstOrDefaultAsync(x => x.ID == request.FineId, cancellationToken);
            if (fine is null) return Result.Failure(FineErrors.FineNotFoundOnUpdateFine);

            if (fine.CommunityId != request.CommunityId)
            {
                var updateCommunity = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.CommunityId, cancellationToken);
                if (updateCommunity is null) return Result.Failure(FineErrors.CommunityNotFoundOnUpdateFine);

                fine = _mapper.Map(request, fine);
                fine.Community = updateCommunity;
                fine.CommunityId = updateCommunity.ID;
                _ = _context.Fine.Update(fine);
                _ = await _context.SaveChangesAsync(cancellationToken);
                var responseCommunityUpdated = _mapper.Map<UpdateFineResponse>(fine);

                return Result.WithResponse(
                new ResponseData { Data = responseCommunityUpdated });
            }

            fine = _mapper.Map(request, fine);
            _ = _context.Fine.Update(fine);
            _ = await _context.SaveChangesAsync(cancellationToken);
            var response = _mapper.Map<UpdateFineResponse>(fine);

            return Result.WithResponse(
            new ResponseData { Data = response });

        }
    }
}
