namespace Portfolio.Api;

public interface IEmailService
{
    /// <summary>
    /// Send an email.
    /// </summary>
    /// <param name="fromAddress"></param>
    /// <param name="name"></param>
    /// <param name="subject"></param>
    /// <param name="plaintextMessageContent"></param>
    /// <returns></returns>
    public Task SendEmail(string fromAddress, string name, string subject, string plaintextMessageContent);
}