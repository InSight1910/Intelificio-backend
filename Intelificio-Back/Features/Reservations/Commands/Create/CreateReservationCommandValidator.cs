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
            .LessThan(DateTime.Today)
            .WithMessage("No es posible realizar una reserva con fechas pasadas.")
            .NotEmpty()
            .WithMessage("La fecha de reserva es obligatorio");
        RuleFor(reservation => reservation.StartTime)
            .NotEmpty()
            .WithMessage("La hora de inicio de la reserva es obligatorio")
            .Must(x => IsTimeGreaterThan(new TimeOnly(8, 0, 0), x))
            .WithMessage("La hora de inicio de la reserva debe ser posterior a las 08:00 AM");
        RuleFor(reservation => reservation.EndTime)
            .NotEmpty()
            .WithMessage("La hora de termino de la reserva es obligatorio")
            .Must(x => IsTimeLessThan(new TimeOnly(23, 30, 0), x))
            .WithMessage("La hora de termino de la reserva no debe exeder las 23:30 PM");
    }

    private bool IsTimeGreaterThan(TimeOnly time, TimeOnly checkTime)
    {
        return checkTime >= time;
    }

    private bool IsTimeLessThan(TimeOnly time, TimeOnly checkTime)
    {
        return checkTime <= time;
    }
}