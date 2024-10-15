using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Guest.Commands.Create
{
    public class CreateGuestCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Rut { get; set; }
        public DateTime? EntryTime { get; set; }
        public string? Plate { get; set; }
        public int? UnitId { get; set; }
    }
}
