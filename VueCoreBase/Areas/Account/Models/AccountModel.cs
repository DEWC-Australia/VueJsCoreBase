using Areas.Account.Models;
using ASPIdentity.Data;
using Controllers.Exceptions;
using Extensions.TokenUrlEncoder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models.Authorisation;
using Models.Base;
using System;
using System.Threading.Tasks;
using Services.Email;
using Extensions.UrlHelperExtensions;
using System.Collections.Generic;
using Controllers.Home;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Areas.Account.Models
{
    public class AccountModel : ModelBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountModel(UserManager<ApplicationUser> userManager) : base()
        {
        }

        public async Task<string> Register(SignInManager<ApplicationUser> _signInManager, IEmailSender _emailSender, 
            RegisterViewModel viewModel, ModelStateDictionary modelState, string scheme, string host)
        {
            this.ValidateModelState(modelState, viewModel);


            ApplicationUser user = null;

            //some type of Database Error
            try
            {
                user = await _userManager.FindByEmailAsync(viewModel.Email);

            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }

            if (user != null)
                throw new ApiException(ExceptionsTypes.RegistrationError, ExceptionErrors.AccountErrors.UserExists);

            user = new ApplicationUser
            {
                Email = viewModel.Email,
                UserName = viewModel.Email,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName
            };

            IdentityResult registerResult = null;

            try
            {
                registerResult = await _userManager.CreateAsync(user, viewModel.Password);

            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }

            if (registerResult == null || !registerResult.Succeeded)
                throw new ApiException(ExceptionsTypes.RegistrationError, registerResult.Errors);

            try
            {
                var resultuserRole = await AuthorisationModel.BuildUser(_userManager, user);
                if (!resultuserRole.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    throw new ApiException(ExceptionsTypes.RegistrationError, resultuserRole.Errors);
                }

                var resultEmployeeRole = await AuthorisationModel.BuildEmployee(_userManager, user);
                if (!resultEmployeeRole.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    throw new ApiException(ExceptionsTypes.RegistrationError, resultEmployeeRole.Errors);
                }
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(user);
                throw new ApiException(ExceptionsTypes.RegistrationError, ex);
            }

            // send registeration email
            try
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = user.Id.ToString().EmailConfirmationLink(code.TokenEncode(), scheme, host);

                await _emailSender.SendEmailConfirmationAsync(viewModel.Email, callbackUrl, user.UserName);
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.EmailError, ex);
            }


            return $"New User({ viewModel.Email}) successfully created.";
        }

        public async Task<Dictionary<string, string>> ConfirmEmailAsync(SignInManager<ApplicationUser> _signinManager, string id, string code)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            var controller = "Home";

            if (id == null || code == null)
            {
                result.Add("controller", controller);
                result.Add("action", nameof(HomeController.Index));
                return result;

            }
                
            string action = nameof(HomeController.Error);

            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    throw new ApiException(ExceptionsTypes.RegistrationError, $"Unable to load user with ID '{id}'.");

                var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, code);

                if (!confirmEmailResult.Succeeded)
                    throw new ApiException(ExceptionsTypes.LoginError, confirmEmailResult.Errors);

                await _signinManager.SignInAsync(user, true);

                action = nameof(HomeController.Index);

            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.LoginError, ex);
            }

            result.Add("controller", controller);
            result.Add("action", action);
            return result;

        }

        public async Task<string> ForgotPassword(IEmailSender _emailSender, ForgotPasswordViewModel viewModel, ModelStateDictionary modelState,
            string scheme, string host)
        {
            var responseMsg = "Password reset email sent. Please follow the instruction to reset your password.";

            this.ValidateModelState(modelState, viewModel);

            ApplicationUser user = null;

            try
            {
                user = await _userManager.FindByEmailAsync(viewModel.Email);

            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }


            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                // Don't reveal that the user does not exist or is not confirmed
                return responseMsg;

            try
            {
                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = user.Id.ToString().ResetPasswordCallbackLink(code.TokenEncode(), scheme, host);

                await _emailSender.SendEmailAsync(viewModel.Email, "Reset Password",
                       $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.EmailError, ex);
            }

            return responseMsg;
        }

        public async Task<string> ResetPassword(ResetPasswordViewModel viewModel, ModelStateDictionary modelState)
        {
            var responseMsg = "Password successful reset.";

            this.ValidateModelState(modelState, viewModel);

            ApplicationUser user = null;

            try
            {
                user = await _userManager.FindByEmailAsync(viewModel.Email);
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }

            if (user == null)
                // Don't reveal that the user does not exist
                return responseMsg;

            try
            {
                var result = await _userManager.ResetPasswordAsync(user, viewModel.Code, viewModel.Password);

                if (result.Succeeded)
                    return responseMsg;
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }

            // Don't reveal error
            return responseMsg;

        }

        public async Task<string> SendVerificationEmail(IEmailSender _emailSender, Guid id, ModelStateDictionary modelState, string scheme, string host)
        {
            var responseMsg = "Please check your email to confirm your email.";

            if (!modelState.IsValid)
                throw new ApiException(ExceptionsTypes.ForgotPassword, modelState);

            ApplicationUser user = null;

            try
            {
                user = await _userManager.FindByIdAsync(id.ToString());
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }


            if (user == null)
                throw new ApiException(ExceptionsTypes.RegistrationError, $"Unable to load user with ID '{id.ToString()}'.");

            try
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = user.Id.ToString().EmailConfirmationLink(code.TokenEncode(), scheme, host);

                var email = user.Email;

                await _emailSender.SendEmailConfirmationAsync(email, callbackUrl, user.UserName);
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.EmailError, ex);
            }

            return responseMsg;
        }

        public async Task<string> ChangePassword(ChangePasswordViewModel viewModel, ModelStateDictionary modelState, ClaimsPrincipal currentUser)
        {
            var responseMsg = "Your password has been changed.";

            this.ValidateModelState(modelState, viewModel);

            ApplicationUser user = null;

            try
            {
                user = await _userManager.GetUserAsync(currentUser);
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }
            
            if (user == null)
                throw new ApiException(ExceptionsTypes.ChangePassword, $"Unable to load user with ID '{_userManager.GetUserId(currentUser)}'.");

            IdentityResult changePasswordResult = null;
            try
            {
                changePasswordResult = await _userManager.ChangePasswordAsync(user, viewModel.OldPassword, viewModel.NewPassword);
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }


            if (!changePasswordResult.Succeeded)
                throw new ApiException(ExceptionsTypes.ChangePassword, changePasswordResult.Errors);

            return responseMsg;
        }

        public async Task<string> SetPassword(SetPasswordViewModel viewModel, ModelStateDictionary modelState, ClaimsPrincipal currentUser)
        {
            var responseMsg = "Password has been set.";

            this.ValidateModelState(modelState, viewModel);

            ApplicationUser user = null;

            try
            {
                user = await _userManager.GetUserAsync(currentUser);
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }


            if (user == null)
            {
                throw new ApiException(ExceptionsTypes.SetPassword, $"Unable to load user with ID '{_userManager.GetUserId(currentUser)}'.");
            }

            IdentityResult addPasswordResult = null;

            try
            {
                addPasswordResult = await _userManager.AddPasswordAsync(user, viewModel.NewPassword);

            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }


            if (!addPasswordResult.Succeeded)
                throw new ApiException(ExceptionsTypes.SetPassword, addPasswordResult.Errors);

            return responseMsg;
        }

        public async Task<string> Update(Guid id, UserDetailsViewModel viewModel, ModelStateDictionary modelState)
        {
            var responseMsg = "Your profile has been updated";

            this.ValidateModelState(modelState, viewModel);

            ApplicationUser user = null;

            try
            {
                user = await _userManager.FindByIdAsync(id.ToString());
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }


            if (user == null)
                throw new ApiException(ExceptionsTypes.UserAccountError, $"Unable to load user with ID '{id.ToString()}'.");

            var email = user.Email;

            if (viewModel.Email != email)
            {
                IdentityResult setEmailResult = null;
                try
                {
                    setEmailResult = await _userManager.SetEmailAsync(user, viewModel.Email);

                }
                catch (Exception ex)
                {
                    throw new ApiException(ExceptionsTypes.DatabaseError, ex);
                }


                if (!setEmailResult.Succeeded)
                    throw new ApiException(ExceptionsTypes.UserAccountError, setEmailResult.Errors);
            }

            var phoneNumber = user.PhoneNumber;
            if (viewModel.PhoneNumber != phoneNumber)
            {
                IdentityResult setPhoneResult = null;
                try
                {
                    setPhoneResult = await _userManager.SetPhoneNumberAsync(user, viewModel.PhoneNumber);

                }
                catch (Exception ex)
                {
                    throw new ApiException(ExceptionsTypes.DatabaseError, ex);
                }

                if (!setPhoneResult.Succeeded)
                    throw new ApiException(ExceptionsTypes.UserAccountError, setPhoneResult.Errors);
            }

            return responseMsg;
        }
    }
}
