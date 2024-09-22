using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Features.Community.Commands.AddUser;
using Backend.Features.Community.Commands.Assign;
using MediatR;
using OfficeOpenXml;

namespace Backend.Features.Community.Commands.AddUserMassive
{
    public class AddUserMassiveCommandHandler : IRequestHandler<AddUserMassiveCommand, Result>
    {
        private readonly IMediator _mediator;

        public AddUserMassiveCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> Handle(AddUserMassiveCommand request, CancellationToken cancellationToken)
        {
            var commands = GetCommands(request.Stream);
            var tasks = new List<Task<Result>>();

            for (int i = 0; i < commands.Count; i += 20)
            {
                var usersToAdd = commands.Skip(i).Take(20).ToList();
                tasks.Add(_mediator.Send(new AddUserCommunityCommand { Users = usersToAdd }));

                _ = await Task.WhenAll(tasks);
            }
            if (tasks.Any(r => r.Result.IsFailure)) return Result.WithErrors(AuthenticationErrors.SignUpMassiveError(tasks.SelectMany(r => r.Result.Errors).ToList()));
            return Result.Success();
        }

        private List<AddUserObject> GetCommands(MemoryStream stream)
        {
            var addUserCommand = new List<AddUserObject>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet excelWorksheet = package.Workbook.Worksheets.First();
                int rowCount = excelWorksheet.Dimension.Rows;
                int columnsCount = excelWorksheet.Dimension.Columns;


                for (int row = 2; row <= rowCount; row++)
                {
                    var user = new AddUserObject
                    {
                        CommunityId = int.Parse(excelWorksheet.Cells[row, 1].Value.ToString()!),
                        UserId = int.Parse(excelWorksheet.Cells[row, 2].Value.ToString()!),
                    };
                    addUserCommand.Add(user);

                }
            }
            return addUserCommand;
        }
    }
}
