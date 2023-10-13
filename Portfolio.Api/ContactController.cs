using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Api;

[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    private readonly IEmailService _emailService;

    public ContactController(IEmailService emailService)
    {
        _emailService = emailService;
    }
    
    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] ContactEmailRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FromAddress) || string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Subject) || string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest("All fields are required.");
        }
        
        await _emailService.SendEmail(request.FromAddress, request.Name, request.Subject, request.Body);
        return NoContent();
    }
}