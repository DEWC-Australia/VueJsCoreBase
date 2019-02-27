using System.Threading.Tasks;


namespace Services.Email
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
