using Backend.Common.Response;
using Backend.Features.Location.Queries.GetCities;
using Backend.Features.Location.Queries.GetCitiesByRegion;
using Backend.Features.Location.Queries.GetCityById;
using Backend.Features.Location.Queries.GetMunicipalities;
using Backend.Features.Location.Queries.GetMunicipalityByCity;
using Backend.Features.Location.Queries.GetMunicipalyById;
using Backend.Features.Location.Queries.GetRegionById;
using Backend.Features.Location.Queries.GetRegions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Location.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController(IMediator mediator) : ControllerBase
    {

        [HttpGet("municipality/{id}")]
        public async Task<IActionResult> GetMunicipalityByID(int id)
        {
            var query = new GetMunicipalityByIdQuery { MunicipalityId = id };
            var result = await mediator.Send(query);
            return result.Match<IActionResult>(
                onSuccess: (result) => Ok(result),
                onFailure: NotFound)
                ;
        }

        [HttpGet("municipality")]
        public async Task<IActionResult> GetMunicipality()
        {
            var query = new GetMunicipalitiesQuery();
            var result = await mediator.Send(query);
            return result.Match<IActionResult>(
                onSuccess: (result) => Ok(result),
                onFailure: NotFound)
                ;
        }

        [HttpGet("region")]
        public async Task<IActionResult> GetRegion()
        {
            var query = new GetRegionsQuery();
            var result = await mediator.Send(query);
            return result.Match<IActionResult>(
                onSuccess: (result) => Ok(result),
                onFailure: NotFound)
                ;
        }

        [HttpGet("region/{id}")]
        public async Task<IActionResult> GetRegionByID(int id)
        {
            var query = new GetRegionByIdQuery { MunicipalityId = id };
            var result = await mediator.Send(query);
            return result.Match<IActionResult>(
                onSuccess: (result) => Ok(result),
                onFailure: NotFound)
                ;
        }

        [HttpGet("city")]
        public async Task<IActionResult> GetCity()
        {
            var query = new GetCitiesQuery();
            var result = await mediator.Send(query);
            return result.Match<IActionResult>(
                onSuccess: (result) => Ok(result),
                onFailure: NotFound)
                ;
        }

        [HttpGet("city/{id}")]
        public async Task<IActionResult> GetCityByID(int id)
        {
            var query = new GetCityByIdQuery { CityId = id };
            var result = await mediator.Send(query);
            return result.Match<IActionResult>(
                onSuccess: (result) => Ok(result),
                onFailure: NotFound)
                ;
        }

        [HttpGet("region/{regionId}/cities")]
        public async Task<IActionResult> GetCitiesByRegion(int regionId)
        {
            var query = new GetCitiesByRegionQuery { RegionId = regionId };
            var result = await mediator.Send(query);
            return result.Match<IActionResult>(
                onSuccess: (result) => Ok(result),
                onFailure: NotFound)
                ;
        }

        [HttpGet("city/{cityId}/municipalities")]
        public async Task<IActionResult> GetMunicipalitiesByCity(int cityId)
        {
            var query = new GetMunicipalityByCityQuery { CityId = cityId };
            var result = await mediator.Send(query);
            return result.Match<IActionResult>(
                onSuccess: (result) => Ok(result),
                onFailure: NotFound)
                ;
        }
    }
}
