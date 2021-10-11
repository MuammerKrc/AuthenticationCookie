using IdentityWithCookies.EmailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.Helper
{
    public sealed class SingletonHelper
    {
        public static IEmailService _emailService { get; set; }

        private static SingletonHelper instance;
        private static SingletonHelper GetInstance
        {
            get{
                if(instance==null)
                {
                    instance = new SingletonHelper(_emailService);
                }
                return instance;
            }
        }

        public SingletonHelper(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async static void SendEmailToResetPasswordToken(string ToEmail, string emailMessage)
        {
            string Subject = "Krc Identity için email sıfırlama maili";
            await _emailService.SendEmailService(ToEmail, Subject, emailMessage);
        }

    }

}
