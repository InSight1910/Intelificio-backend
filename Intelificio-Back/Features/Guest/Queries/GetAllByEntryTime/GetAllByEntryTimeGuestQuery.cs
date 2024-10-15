using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Guest.Queries.GetAllByEntryTime
{
    public class GetAllByEntryTimeGuestQuery : IRequest<Result>
    {
        public DateTime EntryTime { get; set; }
    }
}
