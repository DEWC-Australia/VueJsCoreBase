using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Areas.Authenication.Models
{
    public class AuthenicationConstants
    {
        public const string LoginProvider = "InternalJWT";
        public const string TokenName = "RefreshToken";

        public class BadRequestMessages
        {
            public const string InvalidCredentials = "Invalid e-mail address and/or password";
            public const string LockedOut = "Your account is locked out";

        }
    }
}
