using FluentValidation;

namespace Backend.Features.Community.Commands.Create
{
    public class CreateCommunityCommandValidator : AbstractValidator<CreateCommunityCommand>
    {
        public CreateCommunityCommandValidator()
        {
            _ = RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull();
            _ = RuleFor(x => x.Address)
                .NotNull()
                .NotEmpty();
            _ = RuleFor(x => x.MunicipalityId)
                .GreaterThanOrEqualTo(1)
                .NotNull()
                .NotEmpty();
        }
    }
}
