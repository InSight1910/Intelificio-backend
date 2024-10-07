using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Buildings.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Buildings.Commands.Update
{
    public class UpdateBuildigHandler : IRequestHandler<UpdateBuildingCommand, Result>
    {

        private readonly IntelificioDbContext _context;
        private readonly ILogger<UpdateBuildigHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateBuildigHandler(IntelificioDbContext context, ILogger<UpdateBuildigHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == request.Id);
            if (building == null) return Result.Failure(BuildingErrors.BuildingNotFoundOnUpdate);

            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.CommunityId);
            if (community == null) return Result.Failure(BuildingErrors.CommunityNotFoundOnUpdate);

            if (request.Floors <= 0) return Result.Failure(BuildingErrors.BuildingWithoutFloorsOnUpdate);

            building = _mapper.Map(request, building);
            _ = _context.Buildings.Update(building);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }

    }
}
