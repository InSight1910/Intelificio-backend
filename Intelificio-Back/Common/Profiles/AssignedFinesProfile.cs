using AutoMapper;
using Backend.Features.AssignedFines.Commands.Create;
using Backend.Features.AssignedFines.Commands.Update;
using Backend.Features.AssignedFines.Queries.GetAssignedFinesById;
using Backend.Features.AssignedFines.Queries.GetAssignedFinesByUnitId;
using Backend.Features.AssignedFines.Queries.GetAssignedFinesByUserId;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class AssignedFinesProfile : Profile
    {
        public AssignedFinesProfile() 
        {
            _ = CreateMap<CreateAssignedFinesCommand, AssignedFine>();
            _ = CreateMap<UpdateAssignedFinesCommand, AssignedFine>();
            _ = CreateMap<AssignedFine, GetAssignedFinesByIdQuery>();
             _ = CreateMap<AssignedFine, GetAssignedFinesByUnitIdQuery>();
            _ = CreateMap<AssignedFine, GetAssignedFinesByUserIdQuery>();
        }
    }
}
