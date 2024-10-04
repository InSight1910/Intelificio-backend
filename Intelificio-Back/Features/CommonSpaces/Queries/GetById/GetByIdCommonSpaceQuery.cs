using Backend.Common.Response;
using MediatR;

namespace Backend.Features.CommonSpaces.Queries.GetById
{
    public class GetByIdCommonSpaceQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }
}
