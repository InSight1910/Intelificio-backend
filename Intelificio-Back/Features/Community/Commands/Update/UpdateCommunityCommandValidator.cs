using FluentValidation;

namespace Backend.Features.Community.Commands.Update
{
    public class UpdateCommunityCommandValidator : AbstractValidator<UpdateCommunityCommand>
    {
        public UpdateCommunityCommandValidator()
        {
            _ = RuleFor(x => x.Id).NotEmpty().NotNull();
            _ = RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("El nombre puede estar en blanco.");
            _ = RuleFor(x => x.Address)
                    .NotEmpty()
                    .WithMessage("La direccion no puede estar en blanco.");
            _ = RuleFor(x => x.MunicipalityId)
                    .GreaterThanOrEqualTo(1)
                    .WithMessage("La municipalidad no puede venir en blanco."); ;
        }
    }
}
