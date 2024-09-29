using Backend.Common.Response;
using Backend.Models;
using MediatR;

namespace Backend.Features.CommonSpaces.Commands.Delete
{
    public class DeleteCommonSpaceCommandHandler : IRequestHandler<DeleteCommonSpaceCommand, Result>
    {
        private readonly IntelificioDbContext _context;

        public DeleteCommonSpaceCommandHandler(IntelificioDbContext context)
        {
            _context = context;
        }

        public Task<Result> Handle(DeleteCommonSpaceCommand request, CancellationToken cancellationToken)
        {
            // TODO: Validar que no existan reservas para el espacio común.
            throw new NotImplementedException();
        }
    }
}
