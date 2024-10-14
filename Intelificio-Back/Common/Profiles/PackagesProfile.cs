using AutoMapper;
using Backend.Features.Packages.Command.Create;
using Backend.Models;
using Backend.Models.Enums;

namespace Backend.Common.Profiles;

public class PackagesProfile : Profile
{
    public PackagesProfile()
    {
        CreateMap<CreatePackageCommand, Package>()
            .ForMember(opt => opt.Status, opt => opt.MapFrom(src => PackageStatus.PENDING));
        CreateMap<Package, CreatePackageCommandResponse>();
    }
}