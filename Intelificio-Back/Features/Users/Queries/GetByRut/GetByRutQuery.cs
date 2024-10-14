using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Users.Queries.GetByRut;

public class GetByRutQuery : IRequest<Result>
{
    public string Rut { get; set; }
}