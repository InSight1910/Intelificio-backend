using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Buildings.Common;
using Backend.Features.Contact.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Contact.Queries.GetByID
{
    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetContactByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetContactByIdQueryHandler(IntelificioDbContext context, ILogger<GetContactByIdQueryHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            var contact = await _context.Contacts.Include(x => x.Community).FirstOrDefaultAsync(x => x.ID == request.Id);

            if (contact is null) return Result.Failure(ContactErrors.ContactNotFoundOnQuery);

            var response = _mapper.Map<GetContactByIdQueryResponse>(contact);

            return Result.WithResponse(new ResponseData()
            {
                Data = response
            });
        }
    }
}
