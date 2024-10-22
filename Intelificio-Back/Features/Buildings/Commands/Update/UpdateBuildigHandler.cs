using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Buildings.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Buildings.Commands.Update
{
    public class UpdateBuildigHandler(IntelificioDbContext context, ILogger<UpdateBuildigHandler> logger, IMapper mapper) : IRequestHandler<UpdateBuildingCommand, Result>
    {

        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<UpdateBuildigHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == request.Id, cancellationToken);
            if (building is null) return Result.Failure(BuildingErrors.BuildingNotFoundOnUpdate);

            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.CommunityId,cancellationToken);
            if (community is null) return Result.Failure(BuildingErrors.CommunityNotFoundOnUpdate);

            if (request.Floors <= 0) return Result.Failure(BuildingErrors.BuildingWithoutFloorsOnUpdate);

            building = _mapper.Map(request, building);
            _ = _context.Buildings.Update(building);
            _ = await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }
}
