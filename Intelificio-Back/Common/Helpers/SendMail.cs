
using Backend.Features.Notification.Common;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Backend.Common.Helpers
{
    public class SendMail
    {
        private readonly SendGridClient _client;

        public SendMail(IConfiguration configuration)
        {
            _client = new SendGridClient(configuration.GetValue<string>("SendGrid:ApiKey"));
        }

        public async Task<SendGrid.Response> SendEmailAsync(string email, string subject, string message)
        {
            var from = new EmailAddress("intelificio@duocuc.cl", "Intelificio");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var result = await _client.SendEmailAsync(msg);
            return result;
        }

        public async Task<SendGrid.Response> SendEmailDinamycAsync(object template, string templateID, string communityName, List<EmailAddress> recipients)
        {   

            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("intelificio@duocuc.cl", communityName + " a través de Intelificio"));
            msg.AddTos(recipients);
            msg.SetTemplateId(templateID);

            msg.SetTemplateData(template);
            var response = await _client.SendEmailAsync(msg);
            return response;

        }

    }
}
