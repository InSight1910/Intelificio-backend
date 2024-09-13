using Backend.Models;

namespace Backend.Features.Building.Queries.GetById
{
    public class GetByIdQueryResponse
    {
        public required string Name { get; set; } 
        public required string CommunityName { get; set; }
        public required int Floors { get; set; }
        public required int Units { get; set; }
    }
}
