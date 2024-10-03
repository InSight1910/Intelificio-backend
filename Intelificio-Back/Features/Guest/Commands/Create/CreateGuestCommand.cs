using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Guest.Commands.Create
{
    public class CreateGuestCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string FirstName { get; set; }
        public required string Rut { get; set; }
        public required DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public required string Plate { get; set; }
        public required int UnitId { get; set; }
    }
}
