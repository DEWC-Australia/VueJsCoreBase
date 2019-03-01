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

        [Description("No Query Validation")]
        DatabaseQuery,
        [Description("Area Model Factory")]
        AreaModelFactory,
        [Description("Database Read Error")]
        DatabaseRead,
        
        [Description("Registration Error")]
        RegistrationError,
        [Description("User Action Error")]
        UserAction,
        [Description("User Input Error")]
        UserInput,
        [Description("File Upload Error")]
        FileUpload,
        [Description("File Download Error")]
        FileDownload,
        [Description("File Delete Error")]
        FileDelete,
        [Description("System Error (Please Report to ePapa using contact us email)")]
        SystemError,
        [Description("Port Visit Error")]
        PortVisit,
        [Description("Parse Failed")]
        ParseFailed,
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
        public class ApiErrors
        {
            public const string DeleteUserButHasDutyRole = "User cannot be deleted as they are assigned a role in an active duty watch";
            public const string PortVisitNotFound = "Port Visit for requested action could not be found.";
            public const string FileUploadNoFile = "File to upload not detected, please retry.";
            public const string FileUploadFailedToWrite = "File upload failed to write to server.";
            public const string FileNotFound = "Requested file not found.";
            public const string FileDeleted = "Requested file has been deleted.";
            public const string FileNotOnServer = "Copy of requested file could not be found on the server.";
            public const string FileFailureReading = "Failure reading requested file.";
            public const string PortVisitDetails = "Port visit details data is incorrectly formatted.";
            public const string PortVisitDutywatch = "Port visit dutywatch data is incorrectly formatted.";
            public const string PortVisitOptions = "Port visit options data is incorrectly formatted.";
            public const string PortVisitDutywatchMemberDelete = "Requested member to delete does not exist in your dutywatch.";
            public const string ParsetoInt = "Input is required to be a non decimal number";
            public const string ParsetoDate = "Input is required to be a date";
            public const string ParseUnitUserRole = "Unit user role is not in the correct format";
            public const string ParseThread = "Thread data is not in the correct format";
            public const string ReportNotExist = "Requested report type does not exist.";
            public const string ThreadTypeNotExist = "Requested thread type does not exist";
            public const string ThreadNotExist = "Requested thread does not exist";
            public const string ThreadReadonly = "You only have readonly access to this thread";
            public const string ThreadDetailUser = "Something has gone wrong. ePapa doesn't know the thread type to fulfil your request";

        }

        public const string DBSaveChanges = "Failed to write to database.";
        public const string DatabaseUserOrganisation = "Selected Organisation does not exist.";
        public const string InvalidLoginAttempt = "Invalid login attempt: User does not exist for the Organisation, please register.";
        public const string AccountRouteError = "Requested Account Area route is not supported";
        public const string GeneralUserFailure = "User has no system roles, failed to assign General User Role";
        public const string UserAlreadyRegistered = "User is already registered";
       
    }

}
