using AutoMapper;
using Backend.Features.Packages.Create;
using Backend.Models;

namespace Backend.Common.Profiles;

public class PackagesProfile : Profile
{
    protected PackagesProfile()
    {
        CreateMap<CreatePackageCommand, Package>();
        CreateMap<Package, CreatePackageCommandResponse>();
    }
}