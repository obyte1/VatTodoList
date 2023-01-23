using System.Net.Mail;
using System.Threading.Tasks;
using VAT_TODOLIST.Models;

namespace VAT_TODOLIST.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessageContent message);
    }
}
