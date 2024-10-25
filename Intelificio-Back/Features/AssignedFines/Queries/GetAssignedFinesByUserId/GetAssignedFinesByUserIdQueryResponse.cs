using Backend.Models.Enums;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesByUserId
{
    public class GetAssignedFinesByUserIdQueryResponse
    {
        public required int AssignedFineID { get; set; }
        public required int FineId { get; set; }
        public required string FineName { get; set; }
        public required decimal FineAmount { get; set; }
        public FineDenomination FineStatus { get; set; }
        public required int UnitId { get; set; }

        public required string UnitNumber { get; set; }

        public required string UnitType { get; set; }

        public required int UnitFloor { get; set; }

        public required string UnitBuildingName { get; set; }
        public required int UnitBuildingId { get; set; }
        public required string EventDate { get; set; }
        public required string Comment { get; set; }

        public required string User { get; set; }
    }
}
