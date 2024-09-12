using Backend.Common.Response;

namespace Backend.Features.Unit.Common
{
    public class UnitErrors
    {
        public static Error UnitNotFound = new Error(
           "Unit.GetByID.UnitNotFound", "La unidad no fue encontrada");
    }
}
