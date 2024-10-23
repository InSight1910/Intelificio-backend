using Backend.Common.Response;

namespace Backend.Features.Community.Common
{
    public class CommunityErrors
    {
        //public static readonly Error InvalidParameters(IEnumerable<string> errors) => new Error(
        //   "Authentication.SignUp.InvalidParameters", "One or more validations failed", errors);
        //public static readonly Error AlreadyCreated = new Error(
        //    "Authentication.SignUp.AlreadyCreated", "User already exist", new List<string> { "The email is already registered in our system." });

        public static readonly Error UserNotFound = new Error
        {
            Code = "Community.GetAllByUser.UserNotFound",
            Message = "El usuario no se encuentra en nuestros registros."
        };
        public static readonly Error CommunityNameAlreadyExist = new Error
        {
            Code = "Community.Create.CommunityAlreadyExist",
            Message = "El nombre de la comunidad ya existe.",
        };
        public static readonly Error CommunityRutAlreadyExist = new Error
        {
            Code = "Community.Create.CommunityAlreadyExist",
            Message = "El RUT de la comunidad ya existe.",
        };
        public static readonly Error CommunityNotFoundGetByID = new Error
        {
            Code = "Community.GetByID.CommunityNotFoundGetByID",
            Message = "La comunidad indicada no se encuentra dentro de nuestros registros."
        };

        public static readonly Error MunicipalityNotFoundCreate = new Error
        {
            Code = "Community.Create.MunicipalityNotFoundCreate",
            Message = "La comuna ingresa no es valida."
        };
        public static readonly Error MunicipalityNotFoundUpdate = new Error
        {
            Code = "Community.Update.MunicipalityNotFoundUpdate",
            Message = "La comuna ingresa no es valida."
        };
        public static readonly Error CommunityNotFoundDelete = new Error
        {
            Code = "Community.Delete.CommunityNotFoundDelete",
            Message = "La comunidad indicada no se encuentra dentro de nuestros registros."
        };
        public static readonly Error CommunityNotFoundUpdate = new Error
        {
            Code = "Community.Update.CommunityNotFoundUpdate",
            Message = "La comunidad indicada no se encuentra dentro de nuestros registros."
        };

        public static readonly Error UserIsNotAssigned = new Error
        {
            Code = "Community.RemoveUser.UserIsNotAssigned",
            Message = "El usuario no se encuentra asignado a la comunidad"
        };

        public static readonly Error UserNotFoundRemoveUser = new Error
        {
            Code = "Community.RemoveUser.UserNotFoundRemoveUser",
            Message = "El usuario no se encuentra registrado en nuestro sistema."
        };
        public static readonly Error CommunityNotFoundRemoveUser = new Error
        {
            Code = "Community.RemoveUser.CommunityNotFoundRemoveUser",
            Message = "La comunidad no se encuentra registrada en el sistema."
        };

        public static readonly Error UserNotFoundAddUser = new Error
        {
            Code = "Community.AddUser.UserNotFoundAddUser",
            Message = "El usuario indicado no se encuentra registrado en nuestro sistema."
        };
        public static readonly Error CommunityNotFoundAddUser = new Error
        {
            Code = "Community.AddUser.CommunityNotFoundAddUser",
            Message = "La comunidad indicada no se encuentra registrada en nuestro sistema."
        };

        public static readonly Error UserAlreadyInCommunity = new Error
        {
            Code = "Community.AddUser.UserAlreadyInCommunity",
            Message = "El usuario ya se encuentra asignado a la comunidad indicada."
        };
        internal static Error AdminNotAdminRole;

        public static  Error AdminNotFoundUpdate { get; internal set; }

        public static  Error HasAssignedBuildingsOnDelete(int buildingNumber) => new Error
        {
            Code = "Community.Delete.HasAssignedBuildingsOnDelete",
            Message = buildingNumber == 1 ? "La comunidad tiene un edificio asignado." : string.Format("La comunidad tiene {0} edificios asignados.", buildingNumber)
        };

        public static  List<Error> AddUserMassive(List<Error> errors) => errors;
    }
}
