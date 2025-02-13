using EMS.DAL.Interface;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace EMS.DAL.Implementation
{
    public class EmailMgr : IEmailMgr
    {
        private readonly IConfiguration _config;

        public EmailMgr(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 465, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}

// Port 587(TLS): Recommended for securely sending emails. Requires the use of Transport Layer Security (TLS) encryption. 
// Port 465(SSL): Used for SMTP connections encrypted with Secure Sockets Layer (SSL). Note that this port is considered less modern than TLS but is still supported.