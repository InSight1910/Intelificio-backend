namespace Backend.Features.Community.Commands.Create
{
    public class CreateCommunityCommandResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string RUT { get; set; }

    }
}
