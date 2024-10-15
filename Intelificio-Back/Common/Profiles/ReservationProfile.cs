using AutoMapper;
using Backend.Features.Reservations.Commands.Create;
using Backend.Features.Reservations.Query.GetReservationsById;
using Backend.Models;

namespace Backend.Common.Profiles;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<Reservation, CreateReservationCommandResponse>();
        CreateMap<Reservation, GetReservationsByIdQueryResponse>();
    }
}