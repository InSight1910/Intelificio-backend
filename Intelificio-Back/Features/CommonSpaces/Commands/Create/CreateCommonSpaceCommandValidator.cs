using FluentValidation;

namespace Backend.Features.CommonSpaces.Commands.Create;

public class CreateCommonSpaceCommandValidator : AbstractValidator<CreateCommonSpaceCommand>
{
    public CreateCommonSpaceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Capacity)
            .GreaterThan(0);
        RuleFor(x => x.Location)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.CommunityId)
            .GreaterThan(0);
    }
}