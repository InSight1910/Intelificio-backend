namespace Backend.Features.CommonSpaces.Queries.GetById
{
    public class GetByIdCommonSpaceQueryResponse
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool IsInMaintenance { get; set; }
    }
}
