﻿namespace Backend.Features.Community.Queries.GetById
{
    public class GetByIdCommunityResponse
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required int MunicipalityId { get; set; }
        public required int CityId { get; set; }
        public required int RegionId { get; set; }

    }
}
