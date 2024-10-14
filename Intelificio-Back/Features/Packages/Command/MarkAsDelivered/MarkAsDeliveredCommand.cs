using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Packages.Command;

public class MarkAsDeliveredCommand : IRequest<Result>
{
    public required int Id { get; set; }
    public required int DeliveredToId { get; set; }
}