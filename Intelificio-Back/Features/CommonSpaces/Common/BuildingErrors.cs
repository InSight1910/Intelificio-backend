using Backend.Common.Response;

namespace Backend.Features.CommonSpaces.Common
{
    public class CommonSpacesErrors
    {
        // Errores de  Create
        public static Error CommonSpaceAlreadyExist = new("CommonSpace.Create.CommonSpaceAlreadyExist", "El espacio comun ya se encuentra registrado.");
        public static Error CommunityNotFoundOnCreate = new("CommonSpace.Create.CommunityNotFoundOnCreate", "Comunidad no fue encontrada.");

        // Errores de  Delete
        public static Error CommonSpaceNotFoundOnDelete = new("CommonSpace.Delete.CommonSpaceNotFound", "No fue posible encontrar el espacio comun indicado.");
        public static Error HasPendingReservationsOnDelete = new("Building.Delete.HasAssignedUnits", "El Edificio tiene unidades asignadas.");

        //// Errores de  Update
        public static Error CommonSpaceNotFoundOnUpdate = new("CommonSpace.Update.CommonSpaceNotFound", "No fue posible encontrar el espacio comun indicado.");
        public static Error CommonSpaceNameAlreadyExistOnUpdate = new("CommonSpace.Update.CommonSpaceNameAlreadyExist", "Ya existe un espacio comun registrado con ese nombre.");

        //// Errores de Query's
        public static Error CommunityNotFoundOnQuery = new("CommonSpace.GetAllByCommunity.CommunityNotFoundOnQuery", "La comunidad indicada no fue encontrada.");
        public static Error CommonSpaceNotFoundOnQuery = new("CommonSpace.GetbyId.CommonSpaceNotFoundOnQuery", "El espacio comun no fue encontrado.");
    }
}
