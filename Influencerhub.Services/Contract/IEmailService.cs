using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influencerhub.Services.Contract
{
    public interface IEmailService
    {
        Task SendEmail(string toEmail, string subject, string body);

        Task SendPasswordResetTokenAsync(string toEmail, string token);
    }

}
