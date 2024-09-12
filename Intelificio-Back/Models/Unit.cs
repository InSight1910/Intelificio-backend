﻿using Backend.Models.Base;

namespace Backend.Models
{
    public class Unit : BaseEntity
    {
        public required string Number { get; set; }
        public required Building Building { get; set; }
        public required UnitType Type { get; set; }
        public ICollection<User> users { get; set; } = new List<User>();
    }
}
