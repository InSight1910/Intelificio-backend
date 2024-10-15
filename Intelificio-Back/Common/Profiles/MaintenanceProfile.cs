using AutoMapper;
using Backend.Features.Maintenance.Queries.GetAllByCommunity;
using Backend.Features.Notification.Commands.Maintenance;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class MaintenanceProfile : Profile
    {

        public MaintenanceProfile()
        {
            _ =CreateMap<MaintenanceCommand, Maintenance>();
            _ = CreateMap<Maintenance, GetAllByCommunityQuery>();
        }

    }
}
