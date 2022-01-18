using System;
using System.Net;
using System.Net.Mail;

namespace Caffe.Models
{
    public class Mail
    {
        private readonly string _mail;
        private SmtpClient _smtp;

        public Mail(string mail, string password, string server, int port)
        {
            _mail = mail;

            _smtp = new SmtpClient(server, port);
            _smtp.Credentials = new NetworkCredential(mail, password);
            _smtp.EnableSsl = true;
        }

        public void SendCode(string acsessUrl, string email)
        {
            MailAddress from = new MailAddress(_mail, "Caffe");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Тест";
            m.Body = $"<h2>Сылка для подтверждения: {acsessUrl}</h2>";

            m.IsBodyHtml = true;
            _smtp.Send(m);
        }
    }
}