using System.Text.Json.Serialization;
using Backend.Common.Helpers;
using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Reservations.Commands.Create;

public class CreateReservationCommand : IRequest<Result>
{
    public int UserId { get; set; }

    [JsonConverter(typeof(JsonDateTimeConverter))]
    public DateTime Date { get; set; }

    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;

    public int CommonSpaceId { get; set; }
}