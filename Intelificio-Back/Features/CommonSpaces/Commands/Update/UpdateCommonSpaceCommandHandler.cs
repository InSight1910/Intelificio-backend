using AutoMapper;
using Backend.Common.Response;
using Backend.Features.CommonSpaces.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.CommonSpaces.Commands.Update;

public class UpdateCommonSpaceCommandHandler : IRequestHandler<UpdateCommonSpaceCommand, Result>
{
    private readonly IntelificioDbContext _context;
    private readonly IMapper _mapper;

    public UpdateCommonSpaceCommandHandler(IntelificioDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateCommonSpaceCommand request, CancellationToken cancellationToken)
    {
        var commonSpace = await _context.CommonSpaces.Where(x => x.ID == request.Id).FirstOrDefaultAsync();
        if (commonSpace == null) return Result.Failure(CommonSpacesErrors.CommonSpaceNotFoundOnUpdate);

        var nameExist = await _context.CommonSpaces.AnyAsync(x => x.Name == request.Name && x.ID != request.Id);
        if (nameExist) return Result.Failure(CommonSpacesErrors.CommonSpaceNameAlreadyExistOnUpdate);

        commonSpace = _mapper.Map(request, commonSpace);


        await _context.SaveChangesAsync();
        return Result.WithResponse(new ResponseData { Data = request });
    }
}