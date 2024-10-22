using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Fine.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Fine.Commands.Create
{
    public class CreateFineHandler(IntelificioDbContext context, ILogger<CreateFineHandler> logger, IMapper mapper) : IRequestHandler<CreateFineCommand, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<CreateFineHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(CreateFineCommand request, CancellationToken cancellationToken)
        {
            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.CommunityId, cancellationToken);
            if (community is null) return Result.Failure(FineErrors.CommunityNotFoundOnCreateFine);

            if (!Enum.IsDefined(typeof(FineDenomination), request.Status))
            {
                return Result.Failure(FineErrors.InvalidFineDenominationOnCreateFine);
            }

            var fine = _mapper.Map<Models.Fine>(request);
            fine.Community = community;

            _ = await _context.Fine.AddAsync(fine, cancellationToken);
            _ = await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }

    }
}
