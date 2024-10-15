using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Contact.Commands.Create;
using Backend.Features.Contact.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Contact.Commands.Update
{
    public class UpdateContactHandler : IRequestHandler<UpdateContactCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<UpdateContactHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateContactHandler(IntelificioDbContext context, ILogger<UpdateContactHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.ID == request.Id);
            if (contact is null) return Result.Failure(ContactErrors.ContactNotFoundOnUpdate);

            contact = _mapper.Map(request, contact);
            _ = _context.Contacts.Update(contact);
            _ = await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
