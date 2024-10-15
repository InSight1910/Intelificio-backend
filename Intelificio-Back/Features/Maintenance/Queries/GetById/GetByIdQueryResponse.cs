namespace Backend.Features.Maintenance.Queries.GetById
{
    public class GetByIdQueryResponse
    {
        public required string StartDate { get; set; }
        public required string EndDate { get; set; }
        public string Comment { get; set; } = string.Empty;
        public required int CommonSpaceID { get; set; }
        public required int CommunityID { get; set; }
        public required string CommonSpaceName { get; set; }
        public required string CommonSpaceLocation { get; set; }
        public bool IsActive { get; set; }

    }
}
