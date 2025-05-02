using NETCore.MailKit.Core;
using NETCore.MailKit.Infrastructure.Internal;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DergiOtomasyon.Service
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "in-v3.mailjet.com";
        private readonly int _smtpPort = 587;
        private readonly string _emailSender = "5c079f740e46fe0e62891fe27ff8c875";
        private readonly string _emailPassword = "f47708d4feef81aae81d39c877985b63";
        public async Task SendAsync(string mailTo, string subject, string message, bool isHtml = false, SenderInfo sender = null)
        {
 
            var smtpClient = new SmtpClient(_smtpServer)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_emailSender, _emailPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mailTo), // Gönderen adresi
                Subject = subject,
                Body = message,
                IsBodyHtml = isHtml,
            };
            mailMessage.To.Add(mailTo);

            await smtpClient.SendMailAsync(mailMessage);
        }
        public void Send(string mailTo, string subject, string message, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string subject, string message, string[] attachments, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string subject, string message, Encoding encoding, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string subject, string message, string[] attachments, Encoding encoding, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string mailCc, string mailBcc, string subject, string message, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string mailCc, string mailBcc, string subject, string message, string[] attachments, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, string[] attachments, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }


        public Task SendAsync(string mailTo, string subject, string message, string[] attachments, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string mailTo, string subject, string message, Encoding encoding, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string mailTo, string subject, string message, string[] attachments, Encoding encoding, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, string[] attachments, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, string[] attachments, Encoding encoding, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }
    }
}
