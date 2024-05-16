using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CompanyManagmentSystem.PL.Services.EmailSender
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public async Task SendAsync(string from, string recipients, string subject, string body)
		{
			var senderEmail = _configuration["EmailSetting:SenderEmail"];
			var senderPassword = _configuration["EmailSetting:SenderPassword"];

			var emailMessage = new MailMessage(from, recipients, subject, body);
			//emailMessage.From = new MailAddress(from);
			//emailMessage.To.Add(recipients);
			//emailMessage.Subject = subject;
			//emailMessage.Body = $"<html><body>{body}<body><html>";
			//emailMessage.IsBodyHtml = true;

			var smtpClient = new SmtpClient(_configuration["EmailSetting:SmtpClientServer"], 587);
			smtpClient.Credentials = new NetworkCredential(senderEmail, "mjys vtml jqpj yhth\r\n");
			smtpClient.EnableSsl = true;
			//{
			//	Credentials = new NetworkCredential(senderEmail, "mjys vtml jqpj yhth\r\n"),
			//	EnableSsl = true
			//};
			await smtpClient.SendMailAsync(emailMessage);
		}
	}
}
