using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.EmailService
{
    public interface IEmailService
    {
        
        Task SendEmailService(string Email, string subject, string htmlmessage);
    }
}
