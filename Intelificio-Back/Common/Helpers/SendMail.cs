
using SendGrid;
using SendGrid.Helpers.Mail;

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
    }
}
