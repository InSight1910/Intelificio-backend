using AutoMapper;
using Backend.Features.Location.Common;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            _ = CreateMap<Municipality, MunicipalityResponse>();
        }
    }
}
