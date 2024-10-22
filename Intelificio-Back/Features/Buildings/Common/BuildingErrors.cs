using Backend.Common.Response;

namespace Backend.Features.Buildings.Common
{
    public class BuildingErrors
    {
        // Errores de  Create
        public static readonly Error BuildingWithoutFloorsOnCreate = new("Building.Create.BuildingWithoutFloorsOnCreate", "El edificio debe tener al menos 1 piso asignado.");
        public static readonly Error CommunityNotFoundOnCreate = new("Building.Create.CommunityNotFoundOnCreate", "Comunidad no fue encontrada.");

        // Errores de  Delete
        public static readonly Error BuildingNotFoundOnDelete = new("Building.Delete.BuildingNotFound", "Edificio no fue encontrado.");
        public static readonly Error HasAssignedUnitsOnDelete = new("Building.Delete.HasAssignedUnits", "El Edificio tiene unidades asignadas.");

        // Errores de  Update
        public static readonly Error BuildingWithoutFloorsOnUpdate = new("Building.Update.BuildingWithoutFloorsOnUpdate", "El edificio debe tener al menos 1 piso asignado.");
        public static readonly Error BuildingNotFoundOnUpdate = new("Building.Update.BuildingNotFoundOnUpdate", "Edificio no fue encontrado.");
        public static readonly Error CommunityNotFoundOnUpdate = new("Building.Update.CommunityNotFoundOnUpdate", "Comunidad no fue encontrada.");

        // Errores de  AddUnit
        public static readonly Error UnitNotFoundOnAddUnit = new("Building.AddUnit.UnitNotFoundOnAddUnit", "Unidad no fue encontrada.");
        public static readonly Error BuildingNotFoundOnAddUnit = new("Building.AddUnit.BuildingNotFoundOnAddUnit", "Edificio no fue encontrado.");
        public static readonly Error UnitAlreadyExistsOnAddUnit = new("Building.AddUnit.UnitAlreadyExistsOnAddUnit", "Unidad ya pertenece al Edificio indicado.");

        // Errores de  RemoveInit
        public static readonly Error BuildingNotFoundOnRemoveUnit = new("Building.RemoveUnit.BuildingNotFoundOnRemoveUnit", "Edificio no fue encontrado.");
        public static readonly Error UnitNotFoundOnRemoveUnit = new("Building.RemoveUnit.UnitNotFoundOnRemoveUnit", "Unidad no fue encontrada.");
        public static readonly Error UnitDoesNotExistInBuildingOnRemoveUnit = new("Building.RemoveUnit.UnitDoesNotExistInBuildingOnRemoveUnit", "La Unidad no existe en edificio indicado.");

        // Errores de Query's
        public static readonly Error CommunityNotFoundOnQuery = new("Building.GetAllByCommunity.CommunityNotFoundOnQuery", "Comunidad no fue encontrada.");
        public static readonly Error BuildingNotFoundOnQuery = new("Building.GetbyId.BuildingNotFoundOnQuery", "Edificio no fue encontrado.");
        public static readonly Error BuildingNameAlreadyExist = new("Building.Create.BuildingNameAlreadyExist", "El nombre del edificio ya existe.");
    }
}
