using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Building.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Building.Commands.Create
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
           
            var checkCommunity = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.CommunityId);

            if(checkCommunity == null) return Result.Failure(BuildingErrors.CommunityNotFound);

            var newBuilding = _mapper.Map<Models.Building>(request);
            
            newBuilding.Community = checkCommunity;

            await _context.Buildings.AddAsync(newBuilding);

            return Result.Success();
           
        }
    }
}
