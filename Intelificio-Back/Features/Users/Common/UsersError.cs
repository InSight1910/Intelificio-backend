using Backend.Common.Response;

namespace Backend.Features.Users.Queries.Common;

public class UsersError
{
    public static Error UserNotFoundOnQuery = new()
    {
        Code = "Users.GetByRut.NotFound",
        Message = "No existen usuarios asociados con ese RUT dentro de la comunidad."
    };

    public static Error CommunityNotFoundOnQuery = new()
    {
        Code = "Users.GetAllByCommunity.NotFound",
        Message = "Comunidad no encontrada"
    };
}