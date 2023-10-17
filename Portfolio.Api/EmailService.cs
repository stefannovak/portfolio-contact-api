using Microsoft.Extensions.Options;
using Portfolio.Api;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Portfolio.Api;

public class EmailService : IEmailService
{
    private readonly IOptions<SendGridOptions> _sendGridOptions;

    public EmailService(IOptions<SendGridOptions> sendGridOptions)
    {
        _sendGridOptions = sendGridOptions;

        Client = new SendGridClient(new SendGridClientOptions
        {
            ApiKey = _sendGridOptions.Value.ApiKey,
            HttpErrorAsException = true,
        });
    }

    private SendGridClient Client { get; }

    public async Task SendEmail(string fromAddress, string name, string subject, string plaintextMessageContent)
    {
        var message = new SendGridMessage
        {
            From = new EmailAddress(_sendGridOptions.Value.FromAddress, "Portfolio Contact Form"),
        };
        message.AddTo(_sendGridOptions.Value.DestinationAddress);
        message.Subject = $"{subject} - {fromAddress}";
        message.PlainTextContent = $"Message from {name}: {plaintextMessageContent}";
        await TrySendEmail(message);
    }

    private async Task TrySendEmail(SendGridMessage message)
    {
        try
        {
            var response = await Client.SendEmailAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"Error sending SendGrid email: {body}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to send email. Error: {e.Message}");
        }
    }
}