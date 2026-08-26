using System.Formats.Asn1;
using MailKit.Net.Smtp;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Common.Models;
using MechanicShop.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MechanicShop.Infrastructure.Services;

public sealed class NotificationService(ILogger<NotificationService> logger,IOptions<EmailSettings> settings) : INotificationService
{
    private readonly EmailSettings emailSettings  = settings.Value;
    private const string Message = "Your vehicle service is complete. You may collect it from the shop at your earliest convenience.";

    // public async Task SendEmailAsync(string to, CancellationToken cancellationToken = default)
    // {
    //     var at = to.IndexOf('@');
    //     var maskedEmail = at > 1
    //         ? to[0] + new string('*', at - 2) + to[at - 1] + to[at..]
    //         : "*****";

    //     logger.LogInformation("[Email] To: {Email} | Message: {Message}", maskedEmail, Message);

    //     // Simulated email send
    //     await Task.CompletedTask;
    // }
    public async Task SendEmailAsync(EmailMessage message,CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.To.Add(MailboxAddress.Parse(message.To));
        email.Sender=MailboxAddress.Parse(emailSettings.From);
        email.From.Add(MailboxAddress.Parse(emailSettings.From));
        email.Subject=message.Subject;

        var bodyBuilder = new BodyBuilder();
        if (message.IsHtml)
        {
            bodyBuilder.HtmlBody=message.Body;
        }
        else
        {
            bodyBuilder.TextBody=message.Body;
        }

        email.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(emailSettings.Host,emailSettings.Port,MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(emailSettings.Username,emailSettings.Password);
        await client.SendAsync(email);
        await client.DisconnectAsync(true);
    }

    public async Task SendSmsAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        var masked = phoneNumber.Length >= 4
            ? new string('*', phoneNumber.Length - 4) + phoneNumber[^4..]
            : "****";

        logger.LogInformation("[SMS] To: {Phone} | Message: {Message}", masked, Message);

        // Simulated SMS send
        await Task.CompletedTask;
    }
}