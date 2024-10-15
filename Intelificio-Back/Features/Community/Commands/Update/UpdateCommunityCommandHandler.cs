using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Community.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Commands.Update
{
    public class UpdateCommunityCommandHandler : IRequestHandler<UpdateCommunityCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UpdateCommunityCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateCommunityCommandHandler(IntelificioDbContext context, UserManager<User> userManager, ILogger<UpdateCommunityCommandHandler> logger, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
        {
            Municipality? municipality = null;
            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.Id);
            if (community == null) return Result.Failure(CommunityErrors.CommunityNotFoundUpdate);

            community = _mapper.Map(request, community);

            if (request.MunicipalityId != null)
            {
                municipality = await _context.Municipality.Include(x => x.City).ThenInclude(x => x.Region).FirstOrDefaultAsync(x => x.ID == request.MunicipalityId);
                if (municipality is null) return Result.Failure(CommunityErrors.MunicipalityNotFoundUpdate);
                community.Municipality = municipality;
            }

            if (request.AdminId > 0)
            {
                var admin = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.AdminId);

                if (admin is null) return Result.Failure(CommunityErrors.AdminNotFoundUpdate);

                var isAdmin = await _userManager.IsInRoleAsync(admin, "Administrador");
                if (!isAdmin) return Result.Failure(CommunityErrors.AdminNotAdminRole);


                var isAlreadyInCommunity = await _context.Community
                    .AnyAsync(c => c.ID == community.ID && c.Users.Any(u => u.Id == admin.Id));

                if (!isAlreadyInCommunity)
                {
                    
                    community.Users.Add(admin);
                }
            }


            _ = _context.Community.Update(community);
            var result = await _context.SaveChangesAsync();
            var response = _mapper.Map<UpdateCommunityCommandResponse>(community);
            if (municipality is not null)
            {
                response.RegionId = municipality!.City.Region.ID;
                response.CityId = municipality.City.ID;
                response.MunicipalityId = municipality.ID;
            }

            return Result.WithResponse(
                new ResponseData { Data = response });
        }
    }
}
