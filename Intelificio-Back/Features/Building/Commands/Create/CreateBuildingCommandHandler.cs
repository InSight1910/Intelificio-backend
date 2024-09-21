using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Building.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Building.Commands.Create
{
    public class CreateBuildingCommandHandler : IRequestHandler<CreateBuildingCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<CreateBuildingCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateBuildingCommandHandler(IntelificioDbContext context, ILogger<CreateBuildingCommandHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateBuildingCommand request, CancellationToken cancellationToken)
        {

            if (request.Floors <= 0) return Result.Failure(BuildingErrors.BuildingWithoutFloorsOnCreate);
           
            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.CommunityId);

            if(community is null) return Result.Failure(BuildingErrors.CommunityNotFoundOnCreate);

            var building = _mapper.Map<Models.Building>(request);
            building.Community = community;

            await _context.Buildings.AddAsync(building);

            return Result.Success();
           
        }
    }
}
