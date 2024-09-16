using Backend.Common.Response;

namespace Backend.Features.Community.Common
{
    public class CommunityErrors
    {
        //public static Error InvalidParameters(IEnumerable<string> errors) => new Error(
        //   "Authentication.SignUp.InvalidParameters", "One or more validations failed", errors);
        //public static Error AlreadyCreated = new Error(
        //    "Authentication.SignUp.AlreadyCreated", "User already exist", new List<string> { "The email is already registered in our system." });

        public static Error UserNotFound = new Error
        {
            Code = "Community.GetAllByUser.UserNotFound",
            Message = "User not found"
        };
        public static Error CommunityAlreadyExist = new Error
        {
            Code = "Community.Create.CommunityAlreadyExist",
            Message = "La comunidad ingresada ya se encuentra registrada."
        };

        public static Error MunicipalityNotFoundCreate = new Error
        {
            Code = "Community.Create.MunicipalityNotFoundCreate",
            Message = "La comuna ingresa no es valida."
        };
        public static Error MunicipalityNotFoundUpdate = new Error
        {
            Code = "Community.Update.MunicipalityNotFoundUpdate",
            Message = "La comuna ingresa no es valida."
        };
        public static Error CommunityNotFoundDelete = new Error
        {
            Code = "Community.Delete.CommunityNotFoundDelete",
            Message = "La comunidad indicada no se encuentra dentro de nuestros registros."
        };
        public static Error CommunityNotFoundUpdate = new Error
        {
            Code = "Community.Update.CommunityNotFoundUpdate",
            Message = "La comunidad indicada no se encuentra dentro de nuestros registros."
        };

        public static Error UserIsNotAssigned = new Error
        {
            Code = "Community.RemoveUser.UserIsNotAssigned",
            Message = "El usuario no se encuentra asignado a la comunidad"
        };

        public static Error UserNotFoundRemoveUser = new Error
        {
            Code = "Community.RemoveUser.UserNotFoundRemoveUser",
            Message = "El usuario no se encuentra registrado en el sistema"
        };
        public static Error CommunityNotFoundRemoveUser = new Error
        {
            Code = "Community.RemoveUser.CommunityNotFoundRemoveUser",
            Message = "La comunidad no se encuentra registrada en el sistema"
        };

        public static Error UserNotFoundAddUser = new Error
        {
            Code = "Community.AddUser.UserNotFoundAddUser",
            Message = "El usuario indicado no se encuentra registrado en nuestro sistema."
        };
        public static Error CommunityNotFoundAddUser = new Error
        {
            Code = "Community.AddUser.CommunityNotFoundAddUser",
            Message = "La comunidad indicada no se encuentra registrada en nuestro sistema."
        };

        public static Error UserAlreadyInCommunity = new Error
        {
            Code = "Community.AddUser.UserAlreadyInCommunity",
            Message = "El usuario ya se encuentra asignado a la comunidad indicada."
        };
    }
}
