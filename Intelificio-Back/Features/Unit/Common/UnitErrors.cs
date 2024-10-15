using Backend.Common.Response;

namespace Backend.Features.Unit.Common
{
    public class UnitErrors
    {
        public static Error UnitNotFoundAddUser = new Error(
            "Unit.AddUser.UnitNotFoundAddUser", "La unidad no fue encontrada");

        public static Error UserNotFound = new Error(
            "Unit.AddUser.UserNotFound", "El usuario no fue encontrado");

        public static Error UserAlreadyAssigned = new Error(
            "Unit.AddUser.UserAlreadyAssigned", "El usuario ya ha sido registrado");

        //-----------------------------------------------------------------------------

        public static Error UnitNotFoundRemoveUser = new Error(
            "Unit.RemoveUser.UnitNotFoundRemoveUser", "La unidad no fue encontrada");

        public static Error UserNotFoundRemoveUser = new Error(
            "Unit.RemoveUser.UserNotFoundRemoveUser", "El usuario no fue encontrado");

        public static Error UserIsNotAssigned = new Error(
            "Unit.RemoveUser.UserIsNotAssigned", "El usuario no ha sido registrado");

        //-----------------------------------------------------------------------------

        public static Error UnitTypeNotFound = new Error(
           "Unit.Create.UnitTypeNotFound", "El tipo de unidad no fue encontrado");

        public static Error BuildingNotFound = new Error(
           "Unit.Create.BuildingNotFound", "El edificio no fue encontrado");

        public static Error UnitAlreadyExists = new Error(
           "Unit.Create.UnitAlreadyExists", "La unidad ya ha sido registrada");

        //-----------------------------------------------------------------------------

        public static Error UnitNotFoundUpdate = new Error(
           "Unit.Update.UnitNotFoundUpdate", "La unidad no fue encontrada");

        public static Error UnitTypeNotFoundUpdate = new Error(
           "Unit.Update.UnitTypeNotFoundUpdate", "El tipo de unidad no fue encontrado");

        public static Error BuildingNotFoundUpdate = new Error(
           "Unit.Update.BuildingNotFoundUpdate", "El edificio no fue encontrado");

        //-----------------------------------------------------------------------------

        public static Error UnitNotFoundDelete = new Error(
           "Unit.Delete.UnitNotFoundDelete", "La unidad no fue encontrada");

        //-----------------------------------------------------------------------------

        public static Error UnitNotFoundGetById = new Error(
           "Unit.GetById.UnitNotFoundGetById", "La unidad no fue encontrada");

        //-----------------------------------------------------------------------------

        public static Error UnitNotFoundGetByUser = new Error(
           "Unit.GetByUser.UnitNotFoundGetByUser", "La unidad no fue encontrada");

        //-----------------------------------------------------------------------------

        public static Error UnitNotFoundGetAllByBuilding = new Error(
           "Unit.GetAllByBuilding.UnitNotFoundGetAllByBuilding", "La unidad no fue encontrada");
    }
}
