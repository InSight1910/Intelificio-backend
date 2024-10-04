namespace Backend.Features.CommonSpaces.Commands.Create
{
    public class CreateCommonSpaceCommandResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
            public int Capacity { get; set; }
        public string Location { get; set; }
        public bool IsInMaintenance { get; set; }

    }
}
