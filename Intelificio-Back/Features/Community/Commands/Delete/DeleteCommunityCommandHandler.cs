﻿using Backend.Common.Response;
using Backend.Features.Community.Commands.Create;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Commands.Delete
{
    public class DeleteCommunityCommandHandler : IRequestHandler<DeleteCommunityCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<CreateCommunityCommandHandler> _logger;

        public DeleteCommunityCommandHandler(IntelificioDbContext context, ILogger<CreateCommunityCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteCommunityCommand request, CancellationToken cancellationToken)
        {
            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.Id);
            if (community == null) return Result.Failure(null);
            _ = _context.Community.Remove(community);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
