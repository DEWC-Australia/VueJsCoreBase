
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Services.Email
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {

            string[] txtMessage = { $"To: {email}", $"Subject: {subject}", $"Message: {message}", " ##########################################################################" };
            File.AppendAllLines("emails.txt", txtMessage);
            
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if(env != "Development")
            {

                SmtpClient client = new SmtpClient("mail.epapa.com.au")
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("epapa.noreply@epapa.com.au", "*blink182")
                };

                MailMessage emailMessage = new MailMessage
                {
                    From = new MailAddress("epapa.noreply@epapa.com.au")
                };

                emailMessage.To.Add(email);
                emailMessage.Body = message;
                emailMessage.Subject = subject;

                client.Send(emailMessage);
            }
            

            return Task.FromResult(0);
        }

    }

}
