using Tennis.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Tennis.Interfaces;

public interface IMailService
{
	Task SendEmailAsync(MailRequest mailRequest);
}

public class MailService : IMailService
{
	private readonly IConfiguration _configuration;

	public MailService(IConfiguration configuration)
	{
		_configuration = configuration; // Inject IConfiguration
	}

	public async Task SendEmailAsync(MailRequest mailRequest)
	{
		try
		{
			// Lấy thông tin cấu hình từ appsettings.json
			var mailSettings = _configuration.GetSection("MailSettings");
			string fromMail = mailSettings["Mail"] ?? throw new ArgumentNullException("From email is null");
			string fromPassword = mailSettings["Password"] ?? throw new ArgumentNullException("Password is null");
			string displayName = mailSettings["DisplayName"] ?? "Default Display Name";
			string host = mailSettings["Host"] ?? "smtp.gmail.com";
			int port = int.Parse(mailSettings["Port"] ?? "587");

			// Log cấu hình SMTP để kiểm tra
			Console.WriteLine($"SMTP Config: Host={host}, Port={port}, EnableSsl=True");

			// Tạo địa chỉ người gửi và người nhận
			var fromAddress = new MailAddress(fromMail, displayName);
			var toAddress = new MailAddress(mailRequest.ToEmail);

			// Cấu hình SMTP client
			using var smtp = new SmtpClient(host, port)
			{
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(fromMail, fromPassword),
				Timeout = 20000
			};

			// Tạo email
			using var message = new MailMessage(fromAddress, toAddress)
			{
				Subject = mailRequest.Subject,
				Body = mailRequest.Body,
				IsBodyHtml = true
			};

			// Gửi email
			await smtp.SendMailAsync(message);
			Console.WriteLine("Email sent successfully.");
		}
		catch (SmtpException smtpEx)
		{
			Console.WriteLine($"SMTP Error: {smtpEx.Message}");
			throw new Exception($"SMTP Error: {smtpEx.Message}", smtpEx);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"General Error: {ex.Message}");
			throw new Exception($"Error sending email: {ex.Message}", ex);
		}
	}
}
