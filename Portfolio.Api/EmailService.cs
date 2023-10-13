using Microsoft.Extensions.Options;
using Portfolio.Api;
using SendGrid;
using SendGrid.Helpers.Mail;


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
            From = new EmailAddress(_sendGridOptions.Value.FromAddress, "Stefan Novak"),
        };
        message.AddTo(fromAddress);
        message.Subject = subject;
        message.PlainTextContent = plaintextMessageContent;
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