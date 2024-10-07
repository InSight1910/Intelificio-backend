using Backend.Models.Base;
using OfficeOpenXml.Utils;

namespace Backend.Models;

public class Attendee : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public required string Rut { get; set; }
    public required int ReservationId { get; set; }
    public Reservation Reservation { get; set; }
}