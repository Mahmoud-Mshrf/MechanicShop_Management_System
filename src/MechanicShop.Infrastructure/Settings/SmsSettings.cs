namespace MechanicShop.Infrastructure.Settings;

public sealed class SmsSettings
{
    public required string AccountSid { get; init; }
    public required string AuthToken { get; init; }
    public required string FromPhoneNumber { get; init; } // Your Twilio number, e.g. "+15017122661"
}