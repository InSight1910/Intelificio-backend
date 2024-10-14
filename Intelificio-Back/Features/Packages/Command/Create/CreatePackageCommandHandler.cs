using AutoMapper;
using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Packages.Create;

public class CreatePackageCommandHandler(IntelificioDbContext context, IMapper mapper)
    : IRequestHandler<CreatePackageCommand, Result>
{
    public async Task<Result> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
    {
        if (!await context.Community.AnyAsync(x => x.ID == request.CommunityId)) return Result.Failure(null);

        if (!await context.Users.AnyAsync(x => x.Id == request.ConciergeId)) return Result.Failure(null);

        if (!await context.Users.AnyAsync(x => x.Id == request.RecipientId)) return Result.Failure(null);

        if (await context.Package.AnyAsync(x => x.TrackingNumber == request.TrackingNumber))
            return Result.Failure(null);

        var package = mapper.Map<Package>(request);


        var result = await context.Package.AddAsync(package);
        await context.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<CreatePackageCommandResponse>(result.Entity);

        return Result.WithResponse(new ResponseData { Data = response });
    }
}