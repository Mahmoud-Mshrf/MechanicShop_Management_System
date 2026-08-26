using MailKit.Net.Smtp;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Common.Models;
using MechanicShop.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace MechanicShop.Infrastructure.Services;

public sealed class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> logger;
    private readonly EmailSettings emailSettings;
    private readonly SmsSettings smsSettings;
    private const string Message = "Your vehicle service is complete. You may collect it from the shop at your earliest convenience.";

    public NotificationService(
        ILogger<NotificationService> logger,
        IOptions<EmailSettings> emailOptions,
        IOptions<SmsSettings> smsOptions)
    {
        this.logger = logger;
        emailSettings = emailOptions.Value;
        smsSettings = smsOptions.Value;

        // Twilio client is configured statically per-process; safe to init once.
        TwilioClient.Init(smsSettings.AccountSid, smsSettings.AuthToken);
    }

    public async Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.To.Add(MailboxAddress.Parse(message.To));
        email.Sender = MailboxAddress.Parse(emailSettings.From);
        email.From.Add(MailboxAddress.Parse(emailSettings.From));
        email.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder();
        if (message.IsHtml)
        {
            bodyBuilder.HtmlBody = message.Body;
        }
        else
        {
            bodyBuilder.TextBody = message.Body;
        }

        email.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(emailSettings.Host, emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
        await client.AuthenticateAsync(emailSettings.Username, emailSettings.Password, cancellationToken);
        await client.SendAsync(email, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

    public async Task SendSmsAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        var masked = phoneNumber.Length >= 4
            ? new string('*', phoneNumber.Length - 4) + phoneNumber[^4..]
            : "****";

        try
        {
            var result = await MessageResource.CreateAsync(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(smsSettings.FromPhoneNumber),
                body: Message);

            logger.LogInformation(
                "[SMS] Sent to {Phone} | Sid: {MessageSid} | Status: {Status}",
                masked,
                result.Sid,
                result.Status);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[SMS] Failed to send to {Phone}", masked);
            throw;
        }
    }
}