using Areas.Account.Controllers;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {


        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string id, string code, string scheme, string host)
        {
            string controller = "api/Account";
            string action = nameof(AccountController.ConfirmEmail);

            return $"{scheme}://{host}/{controller}/{action}?id={id}&code={code}";
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string id, string code, string scheme, string host)
        {
            string controller = "api/Account";
            string action = nameof(AccountController.ResetPassword);

            return $"{scheme}://{host}/{controller}/{action}?id={id}&code={code}";

        }
    }
}
