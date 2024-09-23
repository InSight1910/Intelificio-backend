using AutoMapper;
using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Authentication.Queries.GetAllRoles
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetAllRolesQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllRolesQueryHandler(IntelificioDbContext context, ILogger<GetAllRolesQueryHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Result> Handle(GetAllRolesQuery reques, CancellationToken cancellation)
        {
            var role = await _context.Roles.Select(b => new GetAllRolesQueryResponse
            {
                id = b.Id,
                Name = b.Name
            }).ToListAsync();

            return Result.WithResponse(new ResponseData()
            {
                Data = role
            });
        }
    }
}
