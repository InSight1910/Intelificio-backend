using Backend.Models.Enums;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesById
{
    public class GetAssignedFinesByIdQueryResponse
    {
        public required int AssignedFineID { get; set; }
        public required int FineId { get; set; }
        public required string FineName { get; set; }        
        public required decimal FineAmount { get; set; }
        public FineDenomination FineStatus { get; set; }
        public required int UnitId { get; set; }

        public required string UnitNumber { get; set; }

        public required int UnitFloor { get; set; }

        public required string UnitBuildingName { get; set; }
        public required string EventDate { get; set; }
        public required string Comment { get; set; }

    }
}
