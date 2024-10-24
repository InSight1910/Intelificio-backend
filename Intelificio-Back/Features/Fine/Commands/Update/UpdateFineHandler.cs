using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Fine.Common;
using Backend.Models;
using Backend.Models.Enums;
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

            var assignedfine = await _context.AssignedFines.FirstOrDefaultAsync(x => x.Fine.ID == fine.ID, cancellationToken);
            if (assignedfine is not null) return Result.Failure(FineErrors.FineIsAssociatedToOneOrMoreAsignedFinesOnUpdateFine);

            if (fine.CommunityId != request.CommunityId)
            {
                var updateCommunity = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.CommunityId, cancellationToken);
                if (updateCommunity is null) return Result.Failure(FineErrors.CommunityNotFoundOnUpdateFine);

                fine = _mapper.Map(request, fine);
                fine.Community = updateCommunity;
                fine.CommunityId = updateCommunity.ID;
                _ = _context.Fine.Update(fine);
                _ = await _context.SaveChangesAsync(cancellationToken);
                var responseCommunityUpdated = new UpdateFineResponse
                {
                    FineId = fine.ID,
                    Name = fine.Name,
                    Amount = fine.Amount,
                    Status = fine.Status,
                    CommunityId = fine.CommunityId
                };

                return Result.WithResponse(
                new ResponseData { Data = responseCommunityUpdated });
            }

            if (!Enum.IsDefined(typeof(FineDenomination), request.Status))
            {
                return Result.Failure(FineErrors.InvalidFineDenominationOnUpdateFine);
            }

            fine = _mapper.Map(request, fine);
            _ = _context.Fine.Update(fine);
            _ = await _context.SaveChangesAsync(cancellationToken);
            var response = new UpdateFineResponse{
                FineId = fine.ID,
                Name = fine.Name,
                Amount = fine.Amount,
                Status = fine.Status,
                CommunityId = fine.CommunityId
                };

            return Result.WithResponse(
            new ResponseData { Data = response });

        }
    }
}
