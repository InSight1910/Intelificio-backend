using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Users.GetByRut;

public class GetByRutQueryHandler(IntelificioDbContext context) : IRequestHandler<GetByRutQuery, Result>
{
    public async Task<Result> Handle(GetByRutQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users.Where(x => x.Rut == request.Rut).Select(x => new
        {
            Id = x.Id,
            Name = x.ToString()
        }).FirstOrDefaultAsync();

        if (user is null) return Result.Failure(null);

        return Result.WithResponse(new ResponseData()
        {
            Data = user
        });
    }
}