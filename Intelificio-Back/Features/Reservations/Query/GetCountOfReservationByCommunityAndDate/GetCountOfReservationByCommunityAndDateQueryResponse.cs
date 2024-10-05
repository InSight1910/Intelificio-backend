namespace Backend.Features.Reservations.Query.GetCountOfReservationByCommunityAndDate;

public class GetCountOfReservationByCommunityAndDateQueryResponse
{
    public int day { get; set; }
    public List<CountReservations> countReservations { get; set; }
}

public class CountReservations
{
    public int Status { get; set; }
    public int Count { get; set; }
}