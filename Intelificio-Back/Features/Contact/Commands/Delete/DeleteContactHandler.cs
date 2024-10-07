using Backend.Common.Response;
using Backend.Features.Contact.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Contact.Commands.Delete
{
    public class DeleteContactHandler: IRequestHandler<DeleteContactCommand,Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<DeleteContactHandler> _logger;

        public DeleteContactHandler(IntelificioDbContext context, ILogger<DeleteContactHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.ID == request.Id);

            if (contact is null) return Result.Failure(ContactErrors.ContactNotFoundOnDelete);

            _ = _context.Contacts.Remove(contact);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }

    }
}
