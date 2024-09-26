using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Authentication.Queries.GetAllRoles;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;

namespace Backend.Features.Authentication.Queries.GetAllUserAdmin
{
    public class GetAllUserAdminQueryHandler : IRequestHandler<GetAllUserAdminQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetAllUserAdminQuery> _logger;
        private readonly IMapper _mapper;
        public GetAllUserAdminQueryHandler(IntelificioDbContext context, ILogger<GetAllUserAdminQuery> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Result> Handle(GetAllUserAdminQuery reques, CancellationToken cancellationToken)
        {
            var admins = await _context.Users
                .Where(user => user.Role.Name == "Administrador")
                .Select(u => new GetAllUserAdminQueryResponse
                {
                    id = u.Id,
                    FullName = u.FirstName + " " + u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email,
                    Rut = u.Rut
                }).ToListAsync(cancellationToken: cancellationToken);

            return Result.WithResponse(new ResponseData()
            {
                Data = admins
            });
        }
    }
}
