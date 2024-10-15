using System.Security.Policy;

namespace Backend.Features.Guest.Queries.GetAllByCommunityGuest
{
    public class GetAllByCommunityGuestQueryResponse
    {
        public int Id { get; set; }                     
        public required string FirstName { get; set; }         
        public required string LastName { get; set; }
        public string Rut { get; set; }
        public DateTime EntryTime { get; set; }         
        public string Plate { get; set; }  
        public required string Unit { get; set; } 
    }
}
