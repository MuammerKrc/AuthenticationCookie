using IdentityWithCookies.EmailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.Helper
{
    public  class HelperMethods
    {
        public static IEmailService  _emailService { get; set; }

        public HelperMethods(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async static void SendEmailToResetPasswordToken(string ToEmail,string emailMessage)
        {
            string Subject = "Krc Identity için email sıfırlama maili";
            await _emailService.SendEmailService(ToEmail, Subject, emailMessage);
        }
    }
}
