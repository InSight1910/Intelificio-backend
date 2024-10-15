
using Backend.Features.Notification.Commands.CommonExpenses;
using Backend.Features.Notification.Commands.ConfirmEmail;
using Backend.Features.Notification.Commands.Maintenance;
using Backend.Features.Notification.Commands.MaintenanceCancellation;
using Backend.Features.Notification.Commands.MassUserConfirmationEmail;
using Backend.Features.Notification.Commands.PackageDelivered;
using Backend.Features.Notification.Commands.Reservation.SuccessfulReservation;
using Backend.Features.Notification.Commands.SingleMessage;
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

                if (setDynamicTemplateDataValues  && templates != null && i < templates.Count)
                {
                    msg.SetTemplateData(templates[i], i);
                }
            }

            var response = await _client.SendEmailAsync(msg);
            return response;

        }

        public async Task<SendGrid.Response> SendMultipleSingleEmailToMultipleRecipients(EmailAddress from, List<EmailAddress> recipients, string templateId, List<SingleMessageTemplate> templates)
        {

            var msg = new SendGridMessage();
            msg.SetFrom(from);
            msg.TemplateId = templateId;

            var setDynamicTemplateDataValues = templates != null;

            for (var i = 0; i < recipients.Count; i++)
            {
                msg.AddTo(recipients[i], i);

                if (setDynamicTemplateDataValues && templates != null && i < templates.Count)
                {
                    msg.SetTemplateData(templates[i], i);
                }
            }

            var response = await _client.SendEmailAsync(msg);
            return response;

        }

        public async Task<SendGrid.Response> SendCommondExpenses(EmailAddress from, List<EmailAddress> recipients, string templateId, List<CommonExpensesTemplate> templates)
        {

            var msg = new SendGridMessage();
            msg.SetFrom(from);
            msg.TemplateId = templateId;

            var setDynamicTemplateDataValues = templates != null;

            for (var i = 0; i < recipients.Count; i++)
            {
                msg.AddTo(recipients[i], i);

                if (setDynamicTemplateDataValues && templates != null && i < templates.Count)
                {
                    msg.SetTemplateData(templates[i], i);
                }
            }

            var response = await _client.SendEmailAsync(msg);
            return response;

        }

        public async Task<SendGrid.Response> SendMaintenanceNotificationToMultipleRecipients(EmailAddress from, List<EmailAddress> recipients, string templateId, List<MaintenanceTemplate> templates)
        {

            var msg = new SendGridMessage();
            msg.SetFrom(from);
            msg.TemplateId = templateId;

            var setDynamicTemplateDataValues = templates != null;

            for (var i = 0; i < recipients.Count; i++)
            {
                msg.AddTo(recipients[i], i);

                if (setDynamicTemplateDataValues && templates != null && i < templates.Count)
                {
                    msg.SetTemplateData(templates[i], i);
                }
            }

            var response = await _client.SendEmailAsync(msg);
            return response;

        }

        public async Task<SendGrid.Response> SendSingleEmailConfirmationToMultipleRecipients(EmailAddress from, List<EmailAddress> recipients, string templateId, List<SingleUserConfirmationEmailTemplate> templates)
        {

            var msg = new SendGridMessage();
            msg.SetFrom(from);
            msg.TemplateId = templateId;

            var setDynamicTemplateDataValues = templates != null;

            for (var i = 0; i < recipients.Count; i++)
            {
                msg.AddTo(recipients[i], i);

                if (setDynamicTemplateDataValues && templates != null && i < templates.Count)
                {
                    msg.SetTemplateData(templates[i], i);
                }
            }

            var response = await _client.SendEmailAsync(msg);
            return response;

        }

        public async Task<SendGrid.Response> SendMassEmailConfirmationToMultipleRecipients(EmailAddress from, List<EmailAddress> recipients, string templateId, List<MassUserConfirmationEmailTemplate> templates)
        {

            var msg = new SendGridMessage();
            msg.SetFrom(from);
            msg.TemplateId = templateId;

            var setDynamicTemplateDataValues = templates != null;

            for (var i = 0; i < recipients.Count; i++)
            {
                msg.AddTo(recipients[i], i);

                if (setDynamicTemplateDataValues && templates != null && i < templates.Count)
                {
                    msg.SetTemplateData(templates[i], i);
                }
            }

            var response = await _client.SendEmailAsync(msg);
            return response;

        }

        public async Task<SendGrid.Response> SendSuccessfulReservationToMultipleRecipients(EmailAddress from, List<EmailAddress> recipients, string templateId, List<SuccessfulReservationTemplate> templates)
        {

            var msg = new SendGridMessage();
            msg.SetFrom(from);
            msg.TemplateId = templateId;

            var setDynamicTemplateDataValues = templates != null;

            for (var i = 0; i < recipients.Count; i++)
            {
                msg.AddTo(recipients[i], i);

                if (setDynamicTemplateDataValues && templates != null && i < templates.Count)
                {
                    msg.SetTemplateData(templates[i], i);
                }
            }

            var response = await _client.SendEmailAsync(msg);
            return response;

        }

        public async Task<SendGrid.Response> SendPackageDeliveredNotification(EmailAddress from, List<EmailAddress> recipients, string templateId, List<PackageDeliveredTemplate> templates)
        {

            var msg = new SendGridMessage();
            msg.SetFrom(from);
            msg.TemplateId = templateId;

            var setDynamicTemplateDataValues = templates != null;

            for (var i = 0; i < recipients.Count; i++)
            {
                msg.AddTo(recipients[i], i);

                if (setDynamicTemplateDataValues && templates != null && i < templates.Count)
                {
                    msg.SetTemplateData(templates[i], i);
                }
            }

            var response = await _client.SendEmailAsync(msg);
            return response;

        }

        public async Task<SendGrid.Response> SendMaintenanceCancellationNotificationToMultipleRecipients(EmailAddress from, List<EmailAddress> recipients, string templateId, List<MaintenanceCancellationTemplate> templates)
        {

            var msg = new SendGridMessage();
            msg.SetFrom(from);
            msg.TemplateId = templateId;

            var setDynamicTemplateDataValues = templates != null;

            for (var i = 0; i < recipients.Count; i++)
            {
                msg.AddTo(recipients[i], i);

                if (setDynamicTemplateDataValues && templates != null && i < templates.Count)
                {
                    msg.SetTemplateData(templates[i], i);
                }
            }

            var response = await _client.SendEmailAsync(msg);
            return response;

        }



    }
}
