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


        public GetAllByUserQueryHandler(IntelificioDbContext context, ILogger<GetAllByUserQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetAllByUserQuery request, CancellationToken cancellationToken)
        {
            var checkUser = _context.Users.Any(x => x.Id == request.UserId);

            if (!checkUser) return Result.Failure(CommunityErrors.UserNotFound);
            try
            {
                var communities = await _context.Community
                                          .Include(x => x.Users)
                                          .Where(x => x.Users.Any(user => user.Id == request.UserId))
                                          .Select(x => new GetAllByUserResponse
                                          {
                                              Name = x.Name,
                                              Address = x.Address,
                                              BuildingCount = _context.Buildings.Count(b => b.Community.ID == x.ID),
                                              UnitCount = _context.Units.Count(u => u.Building.Community.ID == x.ID),
                                              AdminName = x.Users.Where(user => user.Role.Name == "Administrador" && user.Communities.Any(c => c.ID == user.Id)).Select(u => string.Format("{0} {1}", u.FirstName, u.LastName)).FirstOrDefault() ?? "Sin Administrador"
                                          })
                                          .ToListAsync();
                return Result.WithResponse(new ResponseData()
                {
                    Data = communities
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las comunidades del usuario");
                return Result.Failure(null);
            }



        }
    }
}
