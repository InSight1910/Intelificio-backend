
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

        //public Task<SendGrid.Response> SendMultipleDynamicEmailToMultipleRecipients
        //                                                                       (EmailAddress from,
        //                                                                       List<EmailAddress> tos,
        //                                                                       string templateId,
        //                                                                       List<object> dynamicTemplateData)
        //{
        //    if (string.IsNullOrWhiteSpace(templateId))
        //    {
        //        throw new ArgumentException($"{nameof(templateId)} is required when creating a dynamic template email.", nameof(templateId));
        //    }

        //    var msg = new SendGridMessage();
        //    msg.SetFrom(from);
        //    msg.TemplateId = templateId;

        //    var setDynamicTemplateDataValues = dynamicTemplateData != null;

        //    for (var i = 0; i < tos.Count; i++)
        //    {
        //        msg.AddTo(tos[i], i);

        //        if (setDynamicTemplateDataValues)
        //        {
        //            msg.SetTemplateData(dynamicTemplateData[i], i);
        //        }
        //    }

        //    return msg;
        //}



    }
}
