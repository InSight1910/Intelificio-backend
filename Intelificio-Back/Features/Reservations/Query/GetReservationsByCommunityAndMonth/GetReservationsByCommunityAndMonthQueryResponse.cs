using Org.BouncyCastle.Asn1.X509;

namespace Backend.Features.Reservations.Query.GetReservationsByCommunityAndMonth;

public class GetReservationsByCommunityAndMonthQueryResponse
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string SpaceName { get; set; }
    public int Status { get; set; }
    public string Date { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
}