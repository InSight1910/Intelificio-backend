using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Community.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Commands.Create
{
    public class CreateCommunityCommandHandler : IRequestHandler<CreateCommunityCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<CreateCommunityCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateCommunityCommandHandler(IntelificioDbContext context, ILogger<CreateCommunityCommandHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateCommunityCommand request, CancellationToken cancellationToken)
        {
            var checkCommunityName = await _context.Community.AnyAsync(x => x.Name == request.Name);
            if (checkCommunityName) return Result.Failure(CommunityErrors.CommunityNameAlreadyExist);

            var checkCommunityRut = await _context.Community.AnyAsync(x => x.Rut == request.RUT);
            if (checkCommunityRut) return Result.Failure(CommunityErrors.CommunityRutAlreadyExist);

            var municipality = await _context.Municipality.FirstOrDefaultAsync(x => x.ID == request.MunicipalityId);

            if (municipality is null) return Result.Failure(CommunityErrors.MunicipalityNotFoundCreate);

            var community = _mapper.Map<Models.Community>(request);
            community.Municipality = municipality;

            var result = await _context.Community.AddAsync(community, cancellationToken);
            var id = await _context.SaveChangesAsync();
            var response = _mapper.Map<CreateCommunityCommandResponse>(result.Entity);


            return Result.WithResponse(new ResponseData { Data = response });
        }
    }
}
