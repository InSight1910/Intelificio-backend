using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Community.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Queries.GetAllByUser
{
    public class GetAllByUserQueryHandler : IRequestHandler<GetAllByUserQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetAllByUserQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllByUserQueryHandler(IntelificioDbContext context, ILogger<GetAllByUserQueryHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetAllByUserQuery request, CancellationToken cancellationToken)
        {
            var checkUser = _context.Users.Any(x => x.Id == request.UserId);

            if (!checkUser) return Result.Failure(CommunityErrors.UserNotFound);

            var communities = await _context.Community
                                      .Where(x => x.Users.Any(user => user.Id == request.UserId))
                                      .Include(x => x.Users)
                                      .Select(x => new GetAllByUserResponse
                                      {
                                          Name = x.Name,
                                          Address = x.Address,
                                          BuildingCount = _context.Buildings.Count(b => b.Community.ID == x.ID),
                                          UnitCount = _context.Units.Count(u => u.Building.Community.ID == x.ID),
                                          AdminName = x.Users.Where(user => user.Role.Name == "Administrador" && user.Communities.Any(c => c.ID == user.Id)).Select(u => string.Format("{0} {1}", u.FirstName, u.LastName)).FirstOrDefault()
                                      })
                                      .ToListAsync();

            return Result.WithResponse(new ResponseData()
            {
                Data = communities
            });
        }
    }
}
