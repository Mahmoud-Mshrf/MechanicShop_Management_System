using MechanicShop.Application.Common.Models;

namespace MechanicShop.Application.Common.Interfaces;

public interface INotificationService
{
    // Task SendEmailAsync(string to, CancellationToken cancellationToken = default);
    Task SendEmailAsync(EmailMessage message,CancellationToken cancellationToken = default);
    Task SendSmsAsync(string phoneNumber, CancellationToken cancellationToken = default);
}