using AutoMapper;
using Backend.Features.Reservations.Commands.Create;
using Backend.Models;

namespace Backend.Common.Profiles;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<Reservation, CreateReservationCommandResponse>();
    }
}