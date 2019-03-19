using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Areas.Account.Models
{
    public class AccountConstants
    {
        public const string RegisterResponseMsgStart = "New User(";
        public const string RegisterResponseMsgEnd = ") successfully created.";
        public const string ForgotPasswordMsg = "Password reset email sent. Please follow the instruction to reset your password.";
        public const string ResetPasswordMsg = "Password successful reset.";
        public const string SendVerificationEmailMsg = "Please check your email to confirm your email.";
        public const string ChangePasswordMsg = "Your password has been changed.";
        public const string SetPasswordMsg = "Password has been set.";
        public const string UpdateMsg = "Your profile has been updated";

    }
}
