using Humanizer;
using SlowlySimulate.Infrastructure;
using SlowlySimulate.Shared.Dto.Email;

namespace SlowlySimulate.Api.Factories
{
    public class EmailFactory : IEmailFactory
    {
        private readonly string baseUrl;
        public EmailFactory(IConfiguration configuration)
        {
            baseUrl = configuration[$"{nameof(SlowlySimulate)}:ApplicationUrl"];
        }

        public EmailMessageDto BuildTestEmail(string recipient)
        {
            var emailMessage = new EmailMessageDto();

            emailMessage.Body = "TestEmail.template"
                .FormatWith(new { baseUrl, user = recipient, testDate = DateTime.Now });

            //emailMessage.Subject = L["TestEmail.subject", recipient];
            emailMessage.Subject = $"Test email from {baseUrl}";
            emailMessage.Body = "Test email completed.";

            return emailMessage;
        }
        public EmailMessageDto GetPlainTextTestEmail(DateTime date)
        {
            var emailMessage = new EmailMessageDto();

            emailMessage.Body = "PlainTextTestEmail.template"
                .FormatWith(new { date });

            emailMessage.Subject = "PlainTextTestEmail.subject," + $"{emailMessage.ToAddresses[0].Name}";

            emailMessage.IsHtml = false;

            return emailMessage;
        }
        public EmailMessageDto BuildNewUserConfirmationEmail(string fullName, string userName, string callbackUrl)
        {
            var emailMessage = new EmailMessageDto();

            emailMessage.Body = "NewUserConfirmationEmail.template"
                .FormatWith(new { baseUrl, name = fullName, userName, callbackUrl });

            emailMessage.Subject = "NewUserConfirmationEmail.subject, " + $"{fullName}";

            return emailMessage;
        }
        public EmailMessageDto BuildNewUserEmail(string fullName, string userName, string emailAddress, string password)
        {
            var emailMessage = new EmailMessageDto();

            emailMessage.Body = "NewUserEmail.template"
                .FormatWith(new { baseUrl, fullName = userName, userName, email = emailAddress, password });

            emailMessage.Subject = "NewUserConfirmationEmail.subject, " + $"{fullName}";

            return emailMessage;
        }
        public EmailMessageDto BuilNewUserNotificationEmail(string creator, string name, string userName, string company, string roles)
        {
            var emailMessage = new EmailMessageDto();
            //placeholder not actually implemented

            emailMessage.Body = "NewUserNotificationEmail.template"
                .FormatWith(new { baseUrl, creator, name, userName, roles, company });

            emailMessage.Subject = "NewUserConfirmationEmail.subject, " + $"{userName}";

            return emailMessage;
        }
        public EmailMessageDto BuildForgotPasswordEmail(string name, string callbackUrl, string token)
        {
            var emailMessage = new EmailMessageDto();

            emailMessage.Body = "ForgotPassword.template"
                .FormatWith(new { baseUrl, name, callbackUrl, token });

            emailMessage.Subject = "NewUserConfirmationEmail.subject, " + $"{name}";

            return emailMessage;
        }
        public EmailMessageDto BuildPasswordResetEmail(string userName)
        {
            var emailMessage = new EmailMessageDto();

            emailMessage.Body = "PasswordReset.template"
                .FormatWith(new { baseUrl, userName });

            emailMessage.Subject = "NewUserConfirmationEmail.subject, " + $"{userName}";

            return emailMessage;
        }
    }
}
