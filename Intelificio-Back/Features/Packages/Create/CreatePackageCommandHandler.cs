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
        var checkConcierge = await context.Users.AnyAsync(x => x.Id == request.ConciergeId);
        if (!checkConcierge) return Result.Failure(null);

        var checkRecipient = await context.Users.AnyAsync(x => x.Id == request.RecipientId);
        if (!checkRecipient) return Result.Failure(null);

        var checkTrackNumber = await context.Package.AnyAsync(x => x.TrackingNumber == request.TrackingNumber);
        if (!checkTrackNumber) return Result.Failure(null);

        var package = mapper.Map<Package>(request);

        var result = await context.Package.AddAsync(package);
        await context.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<CreatePackageCommandResponse>(result.Entity);

        return Result.WithResponse(new ResponseData { Data = response });
    }
}