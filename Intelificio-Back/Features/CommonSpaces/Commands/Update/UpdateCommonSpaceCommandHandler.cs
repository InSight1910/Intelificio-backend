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
    private readonly IMediator _mediator;

    public UpdateCommonSpaceCommandHandler(IntelificioDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Result> Handle(UpdateCommonSpaceCommand request, CancellationToken cancellationToken)
    {
        var commonSpace = await _context.CommonSpaces.Where(x => x.ID == request.Id).FirstOrDefaultAsync();
        if (commonSpace == null) return Result.Failure(CommonSpacesErrors.CommonSpaceNotFoundOnUpdate);

        var nameExist = await _context.CommonSpaces.AnyAsync(x => x.Name == request.Name && x.ID != request.Id);
        if (nameExist) return Result.Failure(CommonSpacesErrors.CommonSpaceNameAlreadyExistOnUpdate);

        commonSpace = _mapper.Map(request, commonSpace);

        await _context.SaveChangesAsync();

        if (request.IsInMaintenance)
        {
            var maintenanceCommand = new MaintenanceCommand
            {
                CommunityID = commonSpace.CommunityId,
                CommonSpaceID = commonSpace.ID,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            var maintenanceResult = await _mediator.Send(maintenanceCommand);
            if (!maintenanceResult.IsSuccess)
            {
                return Result.Failure("Error al enviar la notificación de mantenimiento.");
            }
        }

        return Result.WithResponse(new ResponseData { Data = request });
    }
}