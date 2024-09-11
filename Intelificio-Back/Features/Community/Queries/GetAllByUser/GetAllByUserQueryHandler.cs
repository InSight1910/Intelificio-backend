using AutoMapper;
using Backend.Common.Response;
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

            if (!checkUser) return Result.Failure(null);
            var communities = _context.Users
                                      .Where(x => x.Id == request.UserId)
                                      .Include(x => x.Communities)
                                      .SelectMany(x => x.Communities);
            var responseList = new List<GetAllByUserResponse>();
            foreach (var community in communities)
            {
                var response = _mapper.Map<GetAllByUserResponse>(community);
                var admin = await _context.Users
                                        .Where(x => x.Role.Name == "Administrador")
                                        .Where(x => x.Communities.Any(x => x.ID == community.ID))
                                        .Select(x => string.Format("{0} {1}", x.FirstName, x.LastName))
                                        .FirstOrDefaultAsync();
                response.AdminName = admin;

                responseList.Add(response);
            }


            return Result.WithResponse(new ResponseData()
            {
                Data = responseList
            });
        }
    }
}
