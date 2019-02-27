using Areas.Account.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string id, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "api/Account",
                values: null,
                protocol: scheme) + "?id=" + id + "&code=" + code;
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string id, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "api/Account",
                values: null,
                protocol: scheme) + "?id=" + id + "&code=" + code;
        }
    }
}
