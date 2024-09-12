﻿using AutoMapper;
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

            var communities = await _context.Users
                                      .Where(x => x.Id == request.UserId)
                                      .Include(x => x.Communities)
                                      .SelectMany(x => x.Communities)
                                      .Select(x => new GetAllByUserResponse
                                      {
                                          Name = x.Name,
                                          Address = x.Address,
                                          BuildingCount = _context.Buildings.Count(b => b.Community.ID == x.ID),
                                          UnitCount = _context.Units.Count(u => u.Building.Community.ID == x.ID),
                                          AdminName = _context.Users.Where(x => x.Role.Name == "Administrador" && x.Communities.Any(c => c.ID == x.Id)).Select(u => string.Format("{0} {1}", u.FirstName, u.LastName)).FirstOrDefault()
                                      })
                                      .ToListAsync();

            return Result.WithResponse(new ResponseData()
            {
                Data = communities
            });
        }
    }
}