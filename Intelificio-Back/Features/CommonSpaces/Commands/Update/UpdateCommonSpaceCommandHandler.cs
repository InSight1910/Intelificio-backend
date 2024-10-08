using AutoMapper;
using Backend.Common.Response;
using Backend.Features.CommonSpaces.Common;
using Backend.Features.Notification.Commands.Maintenance;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
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

        if (!request.IsInMaintenance && commonSpace.IsInMaintenance)
        {
            var maitenance = await _context.Maintenances.Where(x => x.CommonSpaceID == request.Id && x.IsActive).FirstOrDefaultAsync();
            if (maitenance is null) return Result.Failure(CommonSpacesErrors.MaintenanceNotFoundOnUpdate);
            maitenance.IsActive = false;
        }

        commonSpace = _mapper.Map(request, commonSpace);
        request.CommunityId = commonSpace.CommunityId;

        var response = _mapper.Map<UpdateCommonSpaceCommandResponse>(commonSpace);
       
        await _context.SaveChangesAsync(cancellationToken);
        return Result.WithResponse(new ResponseData { Data = response });
    }
}