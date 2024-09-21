using FluentValidation;

namespace Backend.Features.Unit.Commands.Update
{
    public class UpdateUnitCommandValidator : AbstractValidator<UpdateUnitCommand>
    {
        public UpdateUnitCommandValidator()
        {
            _ = RuleFor(x => x.Number)
                .NotEmpty()
                .NotNull();
            _ = RuleFor(x => x.Floor)
                .NotNull()
                .NotEmpty();
            _ = RuleFor(x => x.Surface)
                .NotNull()
                .NotEmpty();
            _ = RuleFor(x => x.UnitTypeId)
                .GreaterThanOrEqualTo(1)
                .NotNull()
                .NotEmpty();
            _ = RuleFor(x => x.BuildingId)
                .GreaterThanOrEqualTo(1)
                .NotNull()
                .NotEmpty();
        }
    }
}
