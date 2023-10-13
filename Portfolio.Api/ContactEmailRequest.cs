namespace Portfolio.Api;

public class ContactEmailRequest
{
    public string Name { get; set; }
    
    public string FromAddress { get; set; }

    public string Subject { get; set; }
    
    public string Body { get; set; }
}