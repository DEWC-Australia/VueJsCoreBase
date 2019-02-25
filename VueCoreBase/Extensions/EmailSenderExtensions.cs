using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Services.Email;

namespace Extensions.Email
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link, string username)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Your ePapa Username is {username} \n\nPlease confirm your account by clicking this link: {link}");
        }
    }
}
