using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Constants;
using SlowlySimulate.Shared.Dto.Email;

namespace SlowlySimulate.Infrastructure
{
    public interface IEmailManager
    {
        Task<ApiResponse> SendTestEmail(EmailDto parameters);
        Task<ApiResponse> Receive();
        Task<ApiResponse> QueueEmail(EmailMessageDto emailMessage, EmailType emailType);
        Task<ApiResponse> SendEmail(EmailMessageDto emailMessage);
        List<EmailMessageDto> ReceiveEmail(int maxCount = 10);
        Task<ApiResponse> ReceiveMailImapAsync();
        Task<ApiResponse> ReceiveMailPopAsync(int min = 0, int max = 0);
    }
}