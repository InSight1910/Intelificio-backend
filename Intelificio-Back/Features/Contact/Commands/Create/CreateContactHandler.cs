using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Contact.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Contact.Commands.Create
{
    public class CreateContactHandler : IRequestHandler<CreateContactCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<CreateContactHandler> _logger;
        private readonly IMapper _mapper;

        public CreateContactHandler(IntelificioDbContext context, ILogger<CreateContactHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            var checkPhone = await _context.Contacts.AnyAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (checkPhone) return Result.Failure(ContactErrors.PhoneNumberAlreadyExistOnCreate);

            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.CommunityId);
            if (community is null) return Result.Failure(ContactErrors.CommunityNotFoundOnCreate);


            var contact = _mapper.Map<Models.Contact>(request);
            contact.Community = community;

            _ = await _context.Contacts.AddAsync(contact);
            _ = await _context.SaveChangesAsync();

            return Result.Success();

        }
    }
}
