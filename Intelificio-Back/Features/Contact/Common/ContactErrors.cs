using Backend.Common.Response;

namespace Backend.Features.Contact.Common
{
    public class ContactErrors
    {
        // Errores de  Create
        public static Error CommunityNotFoundOnCreate = new("Contact.Create.CommunityNotFoundOnCreate", "Comunidad no fue encontrada.");

        // Errores de Query's
        public static Error CommunityNotFoundOnQuery = new("Contact.GetAllContactsByCommunity.CommunityNotFoundOnQuery", "Comunidad no fue encontrada.");
    }
}
