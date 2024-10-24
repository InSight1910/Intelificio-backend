using Backend.Common.Response;

namespace Backend.Features.Fine.Common
{
    public class FineErrors
    {
        public static readonly Error CommunityNotFoundOnCreateFine = new(
            "Fine.CreateFine.CommunityNotFoundOnCreateFine", 
            "La comunidad consultada no existe en sistema.");

        public static readonly Error InvalidFineDenominationOnCreateFine = new(
            "Fine.CreateFine.InvalidFineDenominationOnCreateFine",
            "La denominación indicada no existe.");

        public static readonly Error FineNotFoundOnDeleteFine = new(
            "Fine.DeleteFine.FineNotFoundOnDeleteFine",
            "No existe la Multa consultada.");

        public static readonly Error FineIsAssociatedToOneOrMoreAsignedFinesOnDeleteFine = new(
            "Fine.DeleteFine.FineIsAssociatedToOneOrMoreAsignedFinesOnDeleteFine",
            "No es posible eliminar esta multa, ya está cursada.");

        public static readonly Error FineIsAssociatedToOneOrMoreAsignedFinesOnUpdateFine = new(
            "Fine.UpdateFine.FineIsAssociatedToOneOrMoreAsignedFinesOnDeleteFine",
            "No es posible modificar esta multa, ya está cursada.");

        public static readonly Error FineNotFoundOnUpdateFine = new(
            "Fine.UpdateFine.FineNotFoundOnUpdateFine",
            "No se puede eliminar esta multa porque está asociada a una o más multas asignadas.");

        public static readonly Error CommunityNotFoundOnUpdateFine = new(
            "Fine.UpdateFine.CommunityNotFoundOnUpdateFine",
            "La comunidad consultada no existe en sistema.");

        public static readonly Error InvalidFineDenominationOnUpdateFine = new(
            "Fine.UpdateFine.InvalidFineDenominationOnUpdateFine",
            "La denominación indicada no existe.");

        public static readonly Error CommunityNotFoundOnGetAllFinesByCommunityQuery = new(
            "Fine.GetAllFinesByCommunityQuery.CommunityNotFoundOnGetAllFinesByCommunityQuery",
            "La comunidad consultada no existe en sistema.");

        public static readonly Error FineNotFoundOnGetFineById = new(
            "Fine.GetFineById.FineNotFoundOnGetFineById",
            "No existe una multa bajo el Id consultado.");

        public static readonly Error Generico5 = new(
            "Fine.DeleteFine.Generico5",
            "No se puede eliminar esta multa porque está asociada a una o más multas asignadas.");

        public static readonly Error Generico6 = new(
            "Fine.DeleteFine.Generico6",
            "No se puede eliminar esta multa porque está asociada a una o más multas asignadas.");





    }
}
