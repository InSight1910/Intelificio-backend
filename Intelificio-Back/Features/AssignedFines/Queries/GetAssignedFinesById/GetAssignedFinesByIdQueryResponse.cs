namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesById
{
    public class GetAssignedFinesByIdQueryResponse
    {
        public int AssignedFineID { get; set; }
        public int FineId { get; set; }
        public int UnitId { get; set; }
        public DateTime EventDate { get; set; }
        public required string Comment { get; set; }
    }
}
