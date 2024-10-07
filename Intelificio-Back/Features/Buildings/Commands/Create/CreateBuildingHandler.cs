using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Buildings.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Buildings.Commands.Create
{
    public class CreateBuildingHandler : IRequestHandler<CreateBuildingCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<CreateBuildingHandler> _logger;
        private readonly IMapper _mapper;

        public CreateBuildingHandler(IntelificioDbContext context, ILogger<CreateBuildingHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateBuildingCommand request, CancellationToken cancellationToken)
        {
            var checkName = await _context.Buildings.AnyAsync(x => x.Name == request.Name && x.Community.ID == request.CommunityId);
            if (checkName) return Result.Failure(BuildingErrors.BuildingNameAlreadyExist);

            if (request.Floors <= 0) return Result.Failure(BuildingErrors.BuildingWithoutFloorsOnCreate);

            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.CommunityId);

            if (community is null) return Result.Failure(BuildingErrors.CommunityNotFoundOnCreate);

            var building = _mapper.Map<Models.Building>(request);
            building.Community = community;

            _ = await _context.Buildings.AddAsync(building);
            _ = await _context.SaveChangesAsync();


            return Result.Success();

        }
    }
}
