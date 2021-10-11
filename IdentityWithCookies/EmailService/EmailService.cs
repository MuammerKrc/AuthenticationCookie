using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IdentityWithCookies.EmailService
{
    public class EmailService : IEmailService
    {
        private string _host;
        private int _port;
        private bool _enableSSL;
        private string _username;
        private string _password;

        
        public EmailService(int port, string host, bool enableSSL, string username, string password)
        {
            _port = port;
            _host = host;
            _enableSSL = enableSSL;
            _username = username;
            _password = password;
        }
        public  Task SendEmailService(string to, string subject, string htmlmessage)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = _enableSSL,
                Timeout=int.MaxValue
            };

            var mesaj = new MailMessage(_username, to, subject, htmlmessage);
            mesaj.IsBodyHtml = true;

            return client.SendMailAsync(mesaj);
            
        }
    }
}
