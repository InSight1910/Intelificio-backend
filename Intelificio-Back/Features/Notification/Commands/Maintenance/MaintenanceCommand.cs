using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.Maintenance
{
    public class MaintenanceCommand : IRequest<Result>
    {
        public required int CommunityID { get; set; }
        public int BuildingID { get; set; } = 0;
        public int Floor { get; set; } = 0;
        public required int CommonSpaceID { get; set; } = 0;
        public required string StartDate { get; set; }
        public required string EndDate { get; set; }

    }
}
