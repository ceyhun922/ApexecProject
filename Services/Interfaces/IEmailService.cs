namespace ApexWebAPI.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendMessageNotificationAsync(string fullName, string senderEmail, string phoneNumber, string messageBody);
    }
}
