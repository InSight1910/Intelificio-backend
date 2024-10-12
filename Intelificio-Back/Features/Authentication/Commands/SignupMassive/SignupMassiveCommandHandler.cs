using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Authentication.Commands.Signup;
using Backend.Features.Authentication.Common;
using Backend.Features.Notification.Commands.ConfirmEmailTwo;
using Backend.Features.Notification.Commands.MassUserSignUpSummary;
using Backend.Models;
using MediatR;
using OfficeOpenXml;


namespace Backend.Features.Authentication.Commands.SignupMassive
{
    public class SignupMassiveCommandHandler : IRequestHandler<SignupMassiveCommand, Result>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SignupMassiveCommandHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Result> Handle(SignupMassiveCommand request, CancellationToken cancellationToken)
        {
            var users = await GetUsersCommands(request.Stream);
            var tasks = new List<Task<List<Result>>>(); 
            var successfullyCreatedUsers = new List<User>();
            var errorMessages = new List<Error>();
            int usersCreated = 0;
            int usersFailed = 0;
            int totalUserSentToCreate = users.Count();

            for (int i = 0; i < users.Count; i += 20)
            {
                var usersToCreate = users.Skip(i).Take(20).ToList();

                tasks.Add(_mediator.Send(new SignUpCommand
                {
                    Users = usersToCreate,
                    CreatorID = request.CreatorID,
                    CommunityID = request.CommunityID,
                    IsMassive = true
                }, cancellationToken)); 
            }

            // Esperamos todas las tareas
            var results = await Task.WhenAll(tasks);


            
            var allResults = results.SelectMany(r => r).ToList(); 

            
            for (int i = 0; i < allResults.Count; i++)
            {
                var result = allResults[i];
                var userObject = users[i]; 

                if (result.IsSuccess)
                {
                    var user = _mapper.Map<User>(userObject);
                    usersCreated++;
                    successfullyCreatedUsers.Add(user); 
                }
                else
                {

                    usersFailed++;
                    errorMessages.Add(result.Error); 
                }
            }


            if (successfullyCreatedUsers.Count > 0)
            {

                var massUserConfirmationEmailCommand = new MassUserConfirmationEmailCommand
                {
                    Users = successfullyCreatedUsers,
                    CommunityID = request.CommunityID
                };
                var massUserConfirmationEmailResult = await _mediator.Send(massUserConfirmationEmailCommand);
                if (massUserConfirmationEmailResult.IsFailure) return Result.Failure(massUserConfirmationEmailResult.Errors);

                var massUserSignUpSummaryCommand = new MassUserSignUpSummaryCommand
                {
                    CreatorID = request.CreatorID,
                    CommunityID = request.CommunityID,
                    TotalEnviados = totalUserSentToCreate.ToString(),
                    TotalCreados = usersCreated.ToString(),
                    TotalErrores = usersFailed.ToString()
                };
                var massUserSignUpSummaryCommandResult = await _mediator.Send(massUserSignUpSummaryCommand);
                if (massUserSignUpSummaryCommandResult.IsFailure) return Result.Failure(massUserConfirmationEmailResult.Error);

            }


            if (usersFailed > 0)
            {
                return Result.WithErrors(AuthenticationErrors.SignUpMassiveError(errorMessages));
            }

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
                        Password = "",
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
