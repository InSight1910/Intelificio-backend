using System.Diagnostics.CodeAnalysis;

namespace Backend.Features.Guest.Queries.GetById
{
    public class GetByIdGuestQueryResponse
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Rut { get; set; }
        public required DateTime EntryTime { get; set; }
        public required string Plate { get; set; }
        public required int BuildingId { get; set; }

        public required int UnitId { get; set; }
    }
}
