using Backend.Common.Response;
using Backend.Features.Authentication.Commands.Signup;
using Backend.Features.Authentication.Common;
using MediatR;
using OfficeOpenXml;

namespace Backend.Features.Authentication.Commands.SignupMassive
{
    public class SignupMassiveCommandHandler : IRequestHandler<SignupMassiveCommand, Result>
    {
        private readonly IMediator _mediator;

        public SignupMassiveCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> Handle(SignupMassiveCommand request, CancellationToken cancellationToken)
        {
            var users = await GetUsersCommands(request.File);
            var tasks = new List<Task>();

            for (int i = 0; i < users.Count; i += 20)
            {
                var usersToCreate = users.Skip(i).Take(20);
                foreach (var user in usersToCreate)
                {
                    tasks.Add(_mediator.Send(user));
                }
                await Task.WhenAll(tasks);
            }
            return Result.Success();
        }

        private async Task<List<UserObject>> GetUsersCommands(IFormFile file)
        {
            var usersCommand = new List<UserObject>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet excelWorksheet = package.Workbook.Worksheets.First();
                    int rowCount = excelWorksheet.Dimension.Rows;
                    int columnsCount = excelWorksheet.Dimension.Columns;


                    for (int row = 2; row <= rowCount; row++)
                    {
                        var user = new UserObject
                        {
                            FirstName = excelWorksheet.Cells[row, (int)SignUpMassiveColumns.FirstName].Value.ToString()!,
                            LastName = excelWorksheet.Cells[row, (int)SignUpMassiveColumns.LastName].Value.ToString()!,
                            Email = excelWorksheet.Cells[row, (int)SignUpMassiveColumns.Email].Value.ToString()!,
                            Password = "ChangeMe.1234",
                            PhoneNumber = excelWorksheet.Cells[row, (int)SignUpMassiveColumns.PhoneNumber].Value.ToString()!,
                            Role = excelWorksheet.Cells[row, (int)SignUpMassiveColumns.Role].Value.ToString()!,
                            Rut = excelWorksheet.Cells[row, (int)SignUpMassiveColumns.Rut].Value.ToString()!,
                        };
                        usersCommand.Add(user);
                    }
                }
            }
            return usersCommand;
        }

    }
}
