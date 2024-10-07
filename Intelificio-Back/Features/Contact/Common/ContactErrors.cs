using Backend.Common.Response;

namespace Backend.Features.Contact.Common
{
    public class ContactErrors
    {
        // Errores de  Create
        public static Error CommunityNotFoundOnCreate = new("Contact.Create.CommunityNotFoundOnCreate", "Comunidad no fue encontrada.");
        public static Error PhoneNumberAlreadyExistOnCreate = new("Contact.Create.PhoneNumberAlreadyExistOnCreate", "El N° de teléfono ya está registrado.");

        // Errores de  Update
        public static Error ContactNotFoundOnUpdate = new("Contact.Update.ContactNotFoundOnUpdate", "No fue posible actualizar el contacto, no existe su ID.");

        // Errores de  Delete
        public static Error ContactNotFoundOnDelete = new("Contact.Delete.ContactNotFoundOnDelete", "No fue posible eliminar el contacto, no existe su ID.");

        // Errores de Query's
        public static Error CommunityNotFoundOnQuery = new("Contact.GetAllContactsByCommunity.CommunityNotFoundOnQuery", "Comunidad no fue encontrada.");
        public static Error ContactNotFoundOnQuery = new("Contact.GetContactByID.ContactNotFoundOnQuery", "No fue posible encontrar el contacto, no existe su ID.");
    }
}
