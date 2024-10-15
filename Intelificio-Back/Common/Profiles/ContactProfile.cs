using AutoMapper;
using Backend.Features.Contact.Commands.Create;
using Backend.Features.Contact.Commands.Update;
using Backend.Features.Contact.Queries.GetallByCommunity;
using Backend.Features.Contact.Queries.GetByID;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            _ = CreateMap<CreateContactCommand, Contact>();
            _ = CreateMap<UpdateContactCommand, Contact>();
            _ = CreateMap<Contact, GetAllContactsByCommunityQueryResponse>();
            _ = CreateMap<Contact, GetContactByIdQueryResponse>();
        }
    }
}
