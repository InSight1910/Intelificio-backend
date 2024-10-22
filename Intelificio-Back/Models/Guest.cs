using Backend.Models.Base;

namespace Backend.Models;

public class Guest : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Rut { get; set; }
    public DateTime EntryTime { get; set; } = DateTime.UtcNow;
    public required string Plate { get; set; }
    public required int UnitId { get; set; }
    public Unit Unit { get; set; }
}