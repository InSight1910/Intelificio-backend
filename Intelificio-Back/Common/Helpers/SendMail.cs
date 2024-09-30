
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

        public async Task<SendGrid.Response> SendSingleDynamicEmailToSingleRecipientAsync(string email, object template, string templateID)
        {
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("intelificio@duocuc.cl", "Intelificio"));
            msg.AddTo(email);
            msg.SetTemplateId(templateID);
            msg.SetTemplateData(template);

            var response = await _client.SendEmailAsync(msg);
            return response;

        }

        public async Task<SendGrid.Response> SendSingleDynamicEmailToMultipleRecipientsAsync(object template, string templateID, EmailAddress from, List<EmailAddress> recipients)
        {   

            var msg = new SendGridMessage();
            msg.SetFrom(from);
            msg.AddTos(recipients);
            msg.SetTemplateId(templateID);
            msg.SetTemplateData(template);

            var response = await _client.SendEmailAsync(msg);
            return response;

        }

        public async Task<SendGrid.Response> SendMultipleDynamicEmailToMultipleRecipients(EmailAddress from, List<EmailAddress> recipients, string templateId,List<object> templates)
        {

            var msg = new SendGridMessage();
            msg.SetFrom(from);
            msg.TemplateId = templateId;

            var setDynamicTemplateDataValues = templates != null;

            for (var i = 0; i < recipients.Count; i++)
            {
                msg.AddTo(recipients[i], i);

                if (setDynamicTemplateDataValues)
                {
                    msg.SetTemplateData(templates[i], i);
                }
            }

            var response = await _client.SendEmailAsync(msg);
            return response;

        }



    }
}
