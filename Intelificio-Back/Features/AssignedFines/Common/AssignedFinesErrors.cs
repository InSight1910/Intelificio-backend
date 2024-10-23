using Backend.Common.Response;

namespace Backend.Features.AssignedFines.Common
{
    public class AssignedFinesErrors
    {
        public static readonly Error FineNotExistOnCreateAssignedFines = new(
            "AssignedFines.CreateAssignedFines.FineNotExistOnCreateAssignedFines",
            "La multa no existe en el sistema.");

        public static readonly Error UnitNotExistOnCreateAssignedFines = new(
            "AssignedFines.CreateAssignedFines.FineNotExistOnCreateAssignedFines",
            "La unidad no existe en el sistema.");

        public static readonly Error FineOrUnitAreDifferentCommunityOnCreateAssignedFines = new(
            "AssignedFines.CreateAssignedFines.FineOrUnitAreDifferentCommunityOnCreateAssignedFines",
            "La unidad o la multa no están en la misma comunidad.");

        public static readonly Error UnitHasNotUsersOnCreateAssignedFines = new(
            "AssignedFines.CreateAssignedFines.UnitHasNotUsersOnCreateAssignedFines",
            "La unidad no tiene usuarios asignados.");

        public static readonly Error EmailAssignedFineNotSend = new(
            "AssignedFines.CreateAssignedFines.EmailAssignedFineNotSend",
            "No se logró enviar la notificación de multa.");

        public static readonly Error NoOwnersFoundOnCreateAssignedFines = new(
            "Notification.CreateAssignedFines.NoOwnersFoundOnCreateAssignedFines",
            "No fue posible asignar la multa, no encontraron Propietarios a los que se pueda notificar.");

        public static readonly Error AssignedFineNotFoundOnUpdateAssignedFines = new(
            "AssignedFines.UpdateAssignedFines.AssignedFineNotFoundOnUpdateAssignedFines",
            "La multa asignada consultada no existe en sistema.");

        public static readonly Error FineNotFoundOnUpdateAssignedFines = new(
            "AssignedFines.UpdateAssignedFines.FineNotFoundOnUpdateAssignedFines",
            "La multa no existe en el sistema.");

        public static readonly Error UnitNotFoundOnUpdateAssignedFines = new(
            "AssignedFines.UpdateAssignedFines.UnitNotFoundOnUpdateAssignedFines",
            "La unidad no existe en el sistema.");

        public static readonly Error AssignedFineNotFoundOnDeleteAssignedFines = new(
            "AssignedFines.DeleteAssignedFines.AssignedFineNotFoundOnDeleteAssignedFines",
            "La multa asignada consultada no existe en sistema.");

        public static readonly Error AssignedFineNotFoundOnGetAssignedfinesByIdQuery = new(
            "AssignedFines.GetAssignedfinesByIdQuery.AssignedFineNotFoundOnGetAssignedfinesByIdQuery",
            "La multa asignada consultada no existe en sistema.");

        public static readonly Error AssignedFineNotFoundOnGetAssignedFinesByUserIdQuery = new(
            "AssignedFines.GetAssignedFinesByUserIdQuery.AssignedFineNotFoundOnGetAssignedFinesByUserIdQuery",
            "La multa asignada consultada no existe en sistema.");

        public static readonly Error UserNotFoundOnGetAssignedFinesByUserId = new(
            "AssignedFines.GetAssignedFinesByUserId.UserNotFoundOnGetAssignedFinesByUserId",
            "No existe usuario consultado, no fue posible obtener las multas.");

        public static readonly Error AssignedFineNotFoundOnGetAssignedFinesByUnitIdQuery = new(
            "AssignedFines.GetAssignedFinesByUnitIdQuery.AssignedFineNotFoundOnGetAssignedFinesByUnitIdQuery",
            "La multa asignada consultada no existe en sistema.");

        public static readonly Error UnitNotFoundOnGetAssignedfinesByUnitId = new(
            "AssignedFines.GetAssignedfinesByUnitId.UnitNotFoundOnGetAssignedfinesByUnitId",
            "La unidad no existe, no fue posible obtener las multas.");

        public static readonly Error CommunityNotFoundOnGetAllAssignedFinesByCommunity = new(
            "AssignedFines.GetAllAssignedFinesByCommunity.CommunityNotFoundOnGetAllAssignedFinesByCommunity",
            "La comunidad no existe en sistema.");

      
    }
}
