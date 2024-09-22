using FluentValidation;

namespace Backend.Features.Authentication.Commands.SignupMassive
{
    public class SignupMassiveCommandValidator : AbstractValidator<SignupMassiveCommand>
    {
        public SignupMassiveCommandValidator()
        {
            _ = RuleFor(x => x.File)
                .NotNull().WithMessage("Se require la carga de un archivo.")
                .Must(BeAValidFile).WithMessage("El archivo debe ser de tipo XLSX o CSV.")
                .Must(HaveValidSize).WithMessage("El archivo debe ser menor a 5MB y no puede estar vacio.");
        }

        private bool BeAValidFile(IFormFile file)
        {
            if (file == null) return false;

            var extension = Path.GetExtension(file.FileName).ToLower();

            return extension.Equals(".csv") || extension.Equals(".xlsx");
        }

        // Validate file size (5MB limit)
        private bool HaveValidSize(IFormFile file)
        {
            if (file == null) return false;

            const int maxFileSize = 5 * 1024 * 1024; // 5MB
            return file.Length <= maxFileSize && file.Length > 0;
        }
    }
}
