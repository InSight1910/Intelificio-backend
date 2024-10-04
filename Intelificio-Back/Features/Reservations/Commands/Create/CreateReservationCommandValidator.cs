using Backend.Models;
using FluentValidation;

namespace Backend.Features.Reservations.Commands.Create;

public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(reservation => reservation.UserId)
            .GreaterThan(0)
            .WithMessage("El usuario no es valido.")
            .NotEmpty()
            .WithMessage("El usuario es obligatorio");
        RuleFor(reservation => reservation.Date)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("No es posible realizar una reserva con fechas pasadas.")
            .NotEmpty()
            .WithMessage("La fecha de reserva es obligatorio");
        RuleFor(reservation => reservation.StartTime)
            .NotEmpty()
            .WithMessage("La hora de inicio de la reserva es obligatorio")
            .Must(x => IsTimeGreaterThan(new TimeSpan(8, 0, 0), x))
            .WithMessage("La hora de inicio de la reserva debe ser posterior a las 08:00 AM");
        RuleFor(reservation => reservation.EndTime)
            .NotEmpty()
            .WithMessage("La hora de termino de la reserva es obligatorio")
            .Must(x => IsTimeLessThan(new TimeSpan(23, 30, 0), x))
            .WithMessage("La hora de termino de la reserva no debe exeder las 23:30 PM");
        RuleFor(x => x).Must(x => x.EndTime > x.StartTime)
            .WithMessage("Tiempo de termino no puede ser antes del inicio de la hora de la reserva.");
    }

    private bool IsTimeGreaterThan(TimeSpan time, TimeSpan checkTime)
    {
        return checkTime >= time;
    }

    private bool IsTimeLessThan(TimeSpan time, TimeSpan checkTime)
    {
        return checkTime <= time;
    }
}