using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.Package
{
    public class PackageCommand : IRequest<Result>
    {
        public required int PackageID { get; set; }
    }
}
