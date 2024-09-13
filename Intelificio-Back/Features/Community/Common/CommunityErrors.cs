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
            Message = "La comunidad ingresada ya se encuentra registrada"
        };

        public static Error MunicipalityNotFound = new Error
        {
            Code = "Community.Create.MunicipalityNotFound",
            Message = "Comuna ingresa no es valida."
        };
        public static Error CommunityNotFoundDelete = new Error
        {
            Code = "Community.Delete.CommunityNotFoundDelete",
            Message = "La comunidad indicada no es valida"
        };
    }
}
