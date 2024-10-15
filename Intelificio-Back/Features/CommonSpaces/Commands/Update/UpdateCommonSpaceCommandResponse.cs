namespace Backend.Features.CommonSpaces.Commands.Update
{
    public class UpdateCommonSpaceCommandResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public bool IsInMaintenance { get; set; }

    }
}
