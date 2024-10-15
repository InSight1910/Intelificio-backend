using AutoMapper;
using Backend.Features.Attendees.Commands.Create;
using Backend.Models;

namespace Backend.Common.Profiles;

public class AttendeeProfile : Profile
{
    public AttendeeProfile()
    {
        CreateMap<CreateAttendeeCommand, Attendee>();
    }
}