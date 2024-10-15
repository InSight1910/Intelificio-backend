namespace Backend.Features.Guest.Queries.GetAllByUnit
{
    public class GetAllByUnitGuestQueryResponse
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Rut { get; set; }
        public required DateTime EntryTime { get; set; }
        public required string Plate { get; set; }
        public required int UnitId { get; set; }
    }
}
