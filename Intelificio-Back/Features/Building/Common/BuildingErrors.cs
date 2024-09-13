using Backend.Common.Response;

namespace Backend.Features.Building.Common
{
    public class BuildingErrors
    {
        public static Error CommunityNotFound = new("Building.Create.CommunityNotFound","Comunidad no fue encontrada.");
        public static Error CommunityQueryNotFound = new("Building.GetAllByCommunity.CommunityQueryNotFound", "Comunidad no fue encontrada.");
        public static Error BuildingNotFound = new("Building.Delete.BuildingNotFound", "Edificio no fue encontrado.");
        public static Error BuildingUpdateNotFound = new("Building.Delete.BuildingUpdateNotFound", "Edificio no fue encontrado.");
        public static Error UnitNotFound = new("Building.AddUnit.UnitNotFound", "Unidad no fue encontrada.");
        public static Error BuildingNotFoundAddUnit = new("Building.AddUnit.BuildingNotFoundAddUnit", "Edificio no fue encontrada.");
        public static Error UnitAlreadyExists = new("Building.AddUnit.UnitAlreadyExists", "Unidad ya se encuentra en Edificio.");
    }
}
