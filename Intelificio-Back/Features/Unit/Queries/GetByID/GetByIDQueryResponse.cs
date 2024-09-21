﻿using Backend.Models;

namespace Backend.Features.Unit.Queries.GetByID
{
    public class GetByIDQueryResponse
    {
        public required string UnitType { get; set; }
        public required string Number { get; set; }
        public required string Building { get; set; }
        public required int Floor { get; set; }
        public required float Surface { get; set; }
    }
}
