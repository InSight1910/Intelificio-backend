using AutoMapper;
using Backend.Common.Response;
using Backend.Common.Security;
using Backend.Features.Authentication.Commands.Signup;
using Backend.Features.Authentication.Common;
using Backend.Features.Community.Commands.AddUser;
using Backend.Features.Notification.Commands.ConfirmEmail;
using Backend.Features.Notification.Commands.ConfirmEmailTwo;
using Backend.Features.Notification.Commands.Maintenance;
using Backend.Features.Notification.Commands.MassUserSignUpSummary;
using Backend.Features.Notification.Commands.SingleUserSignUpSummary;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using MySqlX.XDevAPI;
using OfficeOpenXml;
using System.Collections.Generic;

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
            var tasks = new List<Task<List<Result>>>(); // Lista de tareas para almacenar los resultados de cada lote
            var successfullyCreatedUsers = new List<User>();
            var errorMessages = new List<Error>();
            int usersCreated = 0;
            int usersFailed = 0;
            int totalUserSentToCreate = users.Count();

            for (int i = 0; i < users.Count; i += 20)
            {
                var usersToCreate = users.Skip(i).Take(20).ToList();

                // Enviamos el comando para crear usuarios en lotes
                tasks.Add(_mediator.Send(new SignUpCommand
                {
                    Users = usersToCreate,
                    CreatorID = request.CreatorID,
                    CommunityID = request.CommunityID,
                    IsMassive = true
                }, cancellationToken)); // Pasa el CancellationToken aquí si es necesario
            }

            // Esperamos todas las tareas
            var results = await Task.WhenAll(tasks);


            // Combinamos todos los resultados en una única lista de resultados
            var allResults = results.SelectMany(r => r).ToList(); // Combina las listas de Result en una sola

            // Ahora iteramos sobre cada lote de resultados y los usuarios correspondientes
            for (int i = 0; i < allResults.Count; i++)
            {
                var result = allResults[i];
                var userObject = users[i]; // Obtenemos el UserObject correspondiente

                if (result.IsSuccess)
                {
                    // Si el usuario fue creado exitosamente, mapeamos el UserObject a User
                    var user = _mapper.Map<User>(userObject);
                    usersCreated++;
                    successfullyCreatedUsers.Add(user); // Agregamos el usuario creado a la lista
                }
                else
                {
                    // Si falló, agregamos los errores
                    usersFailed++;
                    errorMessages.AddRange(result.Errors); 
                }
            }

            // Si hubo fallos, devolvemos los errores
            if (usersFailed > 0)
            {
                return Result.WithErrors(AuthenticationErrors.SignUpMassiveError(errorMessages));
            }

            // Solo mandar correos a los usuarios que fueron creados exitosamente
            if (successfullyCreatedUsers.Count > 0)
            {
                // Envia el correo al usuario llamando al MassUserConfirmationEmailCommand
                var massUserConfirmationEmailCommand = new MassUserConfirmationEmailCommand
                {
                    Users = successfullyCreatedUsers,
                    CommunityID = request.CommunityID
                };
                var massUserConfirmationEmailResult = await _mediator.Send(massUserConfirmationEmailCommand);
                if (massUserConfirmationEmailResult.IsFailure) return Result.Failure(massUserConfirmationEmailResult.Errors);

                // Envia confirmación cuenta creada al administrador llamando al MassUserSignUpSummaryCommand 
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
                        Password = PasswordGenerator.GenerateSecurePassword(14),
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
