using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends a generic email (HTML content) to a recipient.
        /// </summary>
        /// <param name="to">Recipient email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="htmlBody">HTML body content</param>
        Task SendEmailAsync(string to, string subject, string htmlBody, List<string>? ccEmails = null);
    }
}
