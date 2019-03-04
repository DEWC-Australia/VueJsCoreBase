using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Controllers.Exceptions
{
    public enum ExceptionsTypes
    {
        [Description("Authorisation")]
        Authorisation,
        [Description("Login Error")]
        LoginError,
        [Description("Database Error")]
        DatabaseError,
        [Description("Forgot Password")]
        ForgotPassword,
        [Description("Change Password")]
        ChangePassword,
        [Description("Set Password")]
        SetPassword,
        [Description("Email Error")]
        EmailError,


        [Description("User Account Error")]
        UserAccountError,

        [Description("Registration Error")]
        RegistrationError,

        

        [Description("File Upload Error")]
        FileUpload,
        [Description("File Download Error")]
        FileDownload,
        [Description("File Delete Error")]
        FileDelete,
        [Description("System Error (Please Report using contact us email)")]
        SystemError,

        [Description("Report Error")]
        ReportError,
        [Description("Thread Error")]
        ThreadError,
        [Description("Unit User Error")]
        UnitUserError

    }

    public class ExceptionErrors
    {
        public class AccountErrors
        {
            public const string UserExists = "A user with that e-mail address already exists.";
            
        }
        public class AuthorisationErrors
        {
            public const string NotAuthorised = "You are not authorised to conduct this action.";
        }

        public const string DBSaveChanges = "Failed to write to database.";
        public const string DatabaseUserOrganisation = "Selected Organisation does not exist.";
        public const string InvalidLoginAttempt = "Invalid login attempt: User does not exist for the Organisation, please register.";
        public const string AccountRouteError = "Requested Account Area route is not supported";
        public const string GeneralUserFailure = "User has no system roles, failed to assign General User Role";
        public const string UserAlreadyRegistered = "User is already registered";
       
    }

}
