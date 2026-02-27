using BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient
            {
                Host = _configuration["Smtp:Host"],
                Port = int.Parse(_configuration["Smtp:Port"]),
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    _configuration["Smtp:User"],
                    _configuration["Smtp:Password"]
                )
            };
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody, List<string>? ccEmails = null)
        {
            using var smtpClient = CreateSmtpClient();

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:FromEmail"], "Cortracker HRMS"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);
            if (ccEmails != null)
            {
                foreach (var cc in ccEmails)
                {
                    if (!string.IsNullOrWhiteSpace(cc))
                        mailMessage.CC.Add(cc);
                }
            }

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
