﻿using Backend.Models.Base;

namespace Backend.Models
{
    public class Pet : BaseEntity
    {
        public int UserId { get; set; }
        public int CommunityId { get; set; }
        public required string Name { get; set; }
        public required string Species { get; set; }
        public DateTime BirthDate { get; set; }
        public required string PhotoUrl { get; set; }
        public bool IsActive { get; set; }
        public required Community Community { get; set; }

        public required User User { get; set; }

    }
}
