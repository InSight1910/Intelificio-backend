using Backend.Common.Response;

namespace Backend.Features.Building.Common
{
    public class BuildingErrors
    {
        public static Error CommunityNotFound = new("Building.Create.CommunityNotFound","Comunidad no fue encontrada");
        public static Error CommunityQueryNotFound = new("Building.GetAllByCommunity.CommunityQueryNotFound", "Comunidad no fue encontrada");
    }
}
