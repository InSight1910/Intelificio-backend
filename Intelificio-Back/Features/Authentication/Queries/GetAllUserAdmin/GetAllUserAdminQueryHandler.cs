using AutoMapper;
using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.Features.Authentication.Queries.GetAllUserAdmin
{
    public class GetAllUserAdminQueryHandler : IRequestHandler<GetAllUserAdminQuery, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GetAllUserAdminQuery> _logger;
        private readonly IMapper _mapper;

        public GetAllUserAdminQueryHandler(UserManager<User> userManager, ILogger<GetAllUserAdminQuery> logger, IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetAllUserAdminQuery reques, CancellationToken cancellationToken)
        {

            var admins = await _userManager.GetUsersInRoleAsync("Administrador");
            var response = admins.Select(u => new GetAllUserAdminQueryResponse
            {
                id = u.Id,
                FullName = u.FirstName + " " + u.LastName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                Rut = u.Rut
            }).ToList();

            return Result.WithResponse(new ResponseData()
            {
                Data = response
            });
        }
    }
}
