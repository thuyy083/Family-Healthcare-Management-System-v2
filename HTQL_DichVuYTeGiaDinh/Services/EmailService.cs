using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string body)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Wellness", emailSettings["SenderEmail"]));
        emailMessage.To.Add(new MailboxAddress("", recipientEmail));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };
        emailMessage.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(emailSettings["SMTPServer"], int.Parse(emailSettings["SMTPPort"]), MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("apikey", emailSettings["SenderPassword"]); // "apikey" là username cố định của SendGrid
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
