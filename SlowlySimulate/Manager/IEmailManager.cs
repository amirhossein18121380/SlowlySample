using CrossCuttingConcerns.Models;
using Domain.Constants;
using Domain.Dto.Email;

namespace SlowlySimulate.Manager
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