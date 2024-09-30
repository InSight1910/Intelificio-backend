using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.CommonExpenses
{
    public class CommonExpenseCommand: IRequest<Result>
    {
        public required int CommunityID { get; set; }
    }
}
