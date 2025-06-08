using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Influencerhub.Services.Contract;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmail(string toEmail, string subject, string body)
    {
        var smtpServer = _configuration["Smtp:Server"];
        var smtpPort = int.Parse(_configuration["Smtp:Port"]);
        var smtpUser = _configuration["Smtp:Username"];
        var smtpPass = _configuration["Smtp:Password"];
        var fromEmail = _configuration["Smtp:FromEmail"];

        var message = new MailMessage(fromEmail, toEmail, subject, body);
        message.IsBodyHtml = true;
        using (var client = new SmtpClient(smtpServer, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true
        })
        {
            await client.SendMailAsync(message);
        }
    }

    public async Task SendPasswordResetTokenAsync(string toEmail, string token)
    {
        var mailSettings = _configuration.GetSection("MailSettings");
        var fromEmail = mailSettings["FromEmail"];
        var fromPassword = mailSettings["AppPassword"];
        var fromName = mailSettings["FromName"] ?? "InfluencerHub";

        var fromAddress = new MailAddress(fromEmail, fromName);
        var toAddress = new MailAddress(toEmail);

        string subject = "Influencerhub - Mã xác thực đặt lại mật khẩu";
        string body = $@"
        <p>Chào bạn,</p>
        <p>Bạn vừa yêu cầu đặt lại mật khẩu trên hệ thống <b>Influencerhub</b>.</p>
        <p><b>Mã xác thực đặt lại mật khẩu của bạn là:</b></p>
        <h2 style='color:#1976d2'>{token}</h2>
        <p>Vui lòng nhập mã này vào màn hình đặt lại mật khẩu để tiếp tục.</p>
        <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>
        <p>Trân trọng,<br/>Influencerhub Team</p>
        ";

        using (var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        })
        using (var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        })
        {
            await smtp.SendMailAsync(message);
        }
    }
}
