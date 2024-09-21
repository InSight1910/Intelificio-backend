using Backend.Common.Response;
using Backend.Models;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Unit.Commands.Update
{
    public class UpdateUnitCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int? Id { get; set; }  
        public string? Number { get; set; }  
        public int? Floor { get; set; } 
        public float Surface { get; set; } 
        public int UnitTypeId { get; set; }  
        public int? BuildingId { get; set; } 
    }
}