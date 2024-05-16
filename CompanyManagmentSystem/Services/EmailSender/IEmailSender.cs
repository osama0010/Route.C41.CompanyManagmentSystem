using System.Threading.Tasks;

namespace CompanyManagmentSystem.PL.Services.EmailSender
{
	public interface IEmailSender
	{
		Task SendAsync(string from, string recipients, string subject, string body);
	}
}
