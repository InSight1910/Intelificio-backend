using FluentValidation;

namespace Backend.Features.CommonSpaces.Commands.Update
{
    public class UpdateCommonSpaceCommandValidator : AbstractValidator<UpdateCommonSpaceCommand>
    {
        public UpdateCommonSpaceCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Location).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Capacity).NotEmpty().GreaterThan(0);
            
        }
    }
}
