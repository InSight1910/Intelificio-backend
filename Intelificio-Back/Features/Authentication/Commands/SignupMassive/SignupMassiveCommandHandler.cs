using Backend.Common.Response;
using Backend.Features.Authentication.Commands.Signup;
using Backend.Features.Authentication.Common;
using Backend.Features.Notification.Commands.ConfirmEmail;
using Backend.Features.Notification.Commands.Maintenance;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
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
            var users = await GetUsersCommands(request.Stream);
            var tasks = new List<Task<Result>>();


            for (int i = 0; i < users.Count; i += 20)
            {
                var usersToCreate = users.Skip(i).Take(20).ToList();
                tasks.Add(_mediator.Send(new SignUpCommand { Users = usersToCreate }));

                _ = await Task.WhenAll(tasks);
            }
            if (tasks.Any(r => r.Result.IsFailure)) return Result.WithErrors(AuthenticationErrors.SignUpMassiveError(tasks.SelectMany(r => r.Result.Errors).ToList()));

            var confirmEmailCommand = new List<ConfirmEmailOneCommand>();

            foreach (var userObject in users)
            {
                var user = new User
                {
                    FirstName = userObject.FirstName,
                    LastName = userObject.LastName,
                    Rut = userObject.Rut,
                    Email = userObject.Email
                };
                confirmEmailCommand.Add(new ConfirmEmailOneCommand { Users = new List<User> { user } });
            }

            var confirmEmailResult = await _mediator.Send(confirmEmailCommand);

            return Result.Success();
        }

        private async Task<List<UserObject>> GetUsersCommands(MemoryStream stream)
        {
            var usersCommand = new List<UserObject>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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



            return usersCommand;
        }

       

    }
}
