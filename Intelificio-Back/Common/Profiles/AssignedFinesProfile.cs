using AutoMapper;
using Backend.Features.AssignedFines.Commands.Create;
using Backend.Features.AssignedFines.Commands.Update;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class AssignedFinesProfile : Profile
    {
        public AssignedFinesProfile() 
        {
            _ = CreateMap<CreateAssignedFinesCommand, AssignedFine>();
            _ = CreateMap<UpdateAssignedFinesCommand, AssignedFine>();
            // _ = CreateMap<AssignedFine, GetAssignedFinesByIdQuery>();
            // _ = CreateMap<AssignedFine, GetAssignedFinesByUnitIdQuery>();
            // _ = CreateMap<AssignedFine, GetAssignedFinesByUserIdQuery>();
        }
    }
}
