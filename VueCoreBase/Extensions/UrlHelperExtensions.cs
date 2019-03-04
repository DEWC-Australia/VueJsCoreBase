using Areas.Account.Controllers;

namespace Extensions.UrlHelperExtensions
{
    public static class UrlHelperExtensions
    {


        public static string EmailConfirmationLink(this string id, string code, string scheme, string host)
        {
            string controller = "api/Account";
            string action = nameof(AccountController.ConfirmEmail);

            return $"{scheme}://{host}/{controller}/{action}?id={id}&code={code}";
        }

        public static string ResetPasswordCallbackLink(this string id, string code, string scheme, string host)
        {
            string controller = "api/Account";
            string action = nameof(AccountController.ResetPassword);

            return $"{scheme}://{host}/{controller}/{action}?id={id}&code={code}";

        }
    }
}
