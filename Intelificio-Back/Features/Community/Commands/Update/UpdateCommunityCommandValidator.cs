using FluentValidation;

namespace Backend.Features.Community.Commands.Update
{
    public class UpdateCommunityCommandValidator : AbstractValidator<UpdateCommunityCommand>
    {
        public UpdateCommunityCommandValidator()
        {
            _ = RuleFor(x => x.Id).NotEmpty().NotNull().GreaterThanOrEqualTo(1);
            _ = RuleFor(x => x.Address).NotEmpty();
            _ = RuleFor(x => x.MunicipalityId).NotEmpty();
        }
    }
}
