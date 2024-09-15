using Backend.Common.Response;

namespace Backend.Features.Unit.Common
{
    public class UnitErrors
    {
        public static Error UnitNotFound = new Error(
           "Unit.GetByID.UnitNotFound", "La unidad no fue encontrada");
        public static Error BuildingNotFound = new Error(
            "Building.CreateUnitCommandHandler.BuildingNotFound", "El edificio no fue encontrado");
        public static Error UnitTypeNotFound = new Error(
            "UnitType.CreateUnitCommandHandler.UnitTypeNotFound", "El tipo de unidad no fue encontrado");
        public static Error UserNotFound = new Error(
            "USer.AddUserCommandHandler.UserNotFound", "El usuario no fue encontrado");
    }
}
