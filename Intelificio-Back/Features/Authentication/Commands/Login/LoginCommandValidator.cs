using FluentValidation;
using System.Text.RegularExpressions;

namespace Backend.Features.Authentication.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            _ = RuleFor(x => x.Email)
                .NotEmpty()
                .Must(x =>
                {
                    string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                    return Regex.IsMatch(x, emailPattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));
                });
            _ = RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);
        }
    }
}
