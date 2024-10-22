using Backend.Common.Response;

namespace Backend.Features.Packages.Common;

public class PackageErrors
{
    public static Error PackageNotFoundOnMarkAsDelivered = new()
    {
        Code = "Packages.MarkAsDelivered.PackageNotFound",
        Message = "El fue posible encontrar el paquete."
    };
    public static Error CommunityNotFoundOnMarkAsDelivered = new()
    {
        Code = "Packages.MarkAsDelivered.CommunityNotFoundOnMarkAsDelivered",
        Message = "No existe comunidad consultada."
    };

    public static Error PackageAlreadyDelivered = new()
    {
        Code = "Packages.MarkAsDelivered.PackageAlready",
        Message = "El paquete ya fue entregado."
    };

    public static Error CommunityNotFoundOnCreate = new()
    {
        Code = "Packages.Create.CommunityNotFound",
        Message = "La comunidad no fue encontrada."
    };

    public static Error ConciergeNotFoundOnCreate = new()
    {
        Code = "Packages.Create.ConciergeNotFound",
        Message = "El conserje no fue encontrado."
    };

    public static Error RecipientNotFoundOnCreate = new()
    {
        Code = "Packages.Create.RecipientNotFound",
        Message = "El usuario no fue encontrado en la comunidad."
    };

    public static Error PackageAlreadyExistOnCreate = new()
    {
        Code = "Packages.Create.PackageAlreadyExists",
        Message = "Ya existe un paquete registrado con ese numero."
    };

    public static Error CommunityNotFoundOnQuery = new()
    {
        Code = "Packages.GetByCommunity.CommunityNotFound",
        Message = "La comunidad no fue encontrada."
    };

    public static Error UserNoBelongToCommunity = new()
    {
        Code = "Packages.MarkAsDelivered.UserNoBelongToCommunity",
        Message = "El usuario no fue encontrado dentro de la comunidad."
    };

    public static Error UserNotAuthorizedToRetired = new()
    {
        Code = "Packages.MarkAsDelivered.UserNotAuthorizedToRetired",
        Message = "El usuario no es encuentra autorizado para retirar la encomienda."
    };

    public static Error CommunityNotFoundOnGetByUserQuery = new()
    {
        Code = "Packages.GetByUserQuery.CommunityNotFoundOnGetByUserQuery",
        Message = "No existe comunidad consultada."
    };

    public static Error UserNotExistOnGetByUserQuery = new()
    {
        Code = "Packages.GetByUserQuery.UserNotExistOnGetByUserQuery",
        Message = "No existe Usuario en la comunidad consultada."
    };

    public static Error CommunityNotFoundOnGetByCommunityQuery = new()
    {
        Code = "Packages.CommunityNotFoundOnGetByCommunity.CommunityNotFoundOnGetByCommunityQuery",
        Message = "No existe comunidad consultada."
    };

}