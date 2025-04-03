namespace Tennis.Models;

public class MailRequest
{
	public MailRequest(string toEmail, string subject, string body)
	{
		ToEmail = toEmail;
		Subject = subject;
		Body = body;
	}

	public string ToEmail { get; set; }
	public string Subject { get; set; }
	public string Body { get; set; }
	public List<IFormFile>? Attachments { get; set; }
}

public class MailSettings
{
	public string Mail { get; set; }
	public string DisplayName { get; set; }
	public string Password { get; set; }
	public string Host { get; set; }
	public int Port { get; set; }
}