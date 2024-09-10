﻿using Backend.Models.Base;

namespace Backend.Models
{
    public class UnitType : BaseEntity
    {
        public required string Description {  get; set; }
        public required IEnumerable<Unit> Units { get; set; }
    }
}