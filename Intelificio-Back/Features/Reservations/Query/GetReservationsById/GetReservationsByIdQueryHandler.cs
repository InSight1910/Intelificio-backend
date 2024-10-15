using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Reservations.Common;
using Backend.Features.Reservations.Query.GetReservationsByUser;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Reservations.Query.GetReservationsById
{
    public class GetReservationsByIdQueryHandler: IRequestHandler<GetReservationsByIdQuery,Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetReservationsByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetReservationsByIdQueryHandler(IntelificioDbContext context, ILogger<GetReservationsByIdQueryHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetReservationsByIdQuery request, CancellationToken cancellationToken)
        {

            var reservation = await _context.Reservations.AnyAsync(x => x.ID == request.ReservationId);
            if (!reservation) return Result.Failure(ReservationErrors.ReservationsNotFoundOnQueryByID);

            var result = await _context.Reservations
            .Include(x => x.Spaces)
            .Include(x => x.Attendees)
            .Where(x => x.ID == request.ReservationId)
            .Select(x => new GetReservationsByIdQueryResponse
            {
                Status = (int)x.Status,
                StartTime = TimeOnly.FromTimeSpan(x.StartTime).ToString(@"hh\:mm tt"),
                EndTime = TimeOnly.FromTimeSpan(x.EndTime).ToString(@"hh\:mm tt"),
                SpaceName = x.Spaces.Name,
                Date = x.Date,
                Id = x.ID,
                Attendees = x.Attendees.Count(),
                Location = x.Spaces.Location
            })
            .OrderBy(x => x.Status).FirstOrDefaultAsync(cancellationToken: cancellationToken);


            return Result.WithResponse(new ResponseData() { Data = result });
        }
    }
}
