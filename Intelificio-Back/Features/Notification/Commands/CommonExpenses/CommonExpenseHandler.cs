using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;

namespace Backend.Features.Notification.Commands.CommonExpenses
{
    public class CommonExpenseHandler : IRequestHandler<CommonExpenseCommand ,Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<CommonExpenseHandler> _logger;


        public CommonExpenseHandler(IntelificioDbContext context, ILogger<CommonExpenseHandler> logger, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(CommonExpenseCommand request, CancellationToken cancellationToken)
        {
            //ToDo: Traer los gastos de la comunidad, Nombre Unidad, Monto total a pagar,Fecha limite de pago

            var recipients = new List<EmailAddress>();

            recipients.Add(new EmailAddress(

            //ToDo:  Armar lista de destinatarios 

            //user.User.Email ?? "intelificio@duocuc.cl",
            //$"{user.User.FirstName} {user.User.LastName}"
            ));

            //ToDo:  Falta pasarle el nombre de comunidad
            var from = new EmailAddress("intelificio@duocuc.cl", $"{"NombreCOMUNIDAD"}" + " a través de Intelificio");

            var templates = new List<CommonExpensesTemplate>();

            var tem = new CommonExpensesTemplate
            {
                CommunityName = "",
                Name = "",
                MesAno = "",
                UniteName = "",
                Total = "",
                EndDate = "",
            };
            templates.Add(tem);

            //ToDo:  generar el template por cada unidad

            if (templates == null) return Result.Failure(NotificationErrors.TemplateNotCreated);

            var result = await _sendMail.SendCommondExpenses(
                                                            from,
                                                            recipients,
                                                            TemplatesEnum.SingleMessageIntelificioId,
                                                            templates
            );

            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSent);
            return Result.Success();

        }

    }
}
