/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ePapa.Controllers;
using ePapa.Areas.Accounts.Controllers;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string id, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(ManagerController.ConfirmEmail),
                controller: "Manager",
                values: null,
                protocol: scheme) + "?id=" + id + "&code=" + code;
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string id, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(ManagerController.ResetPassword),
                controller: "Manager",
                values: null,
                protocol: scheme) + "?id=" + id + "&code=" + code;
        }
    }
}
*/