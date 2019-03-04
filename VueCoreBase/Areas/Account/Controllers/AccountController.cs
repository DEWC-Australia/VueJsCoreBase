using Areas.Account.Models;
using ASPIdentity.Data;
using Controllers.Exceptions;
using Extensions.TokenUrlEncoder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Authorisation;
using Services.Email;
using System;
using System.Threading.Tasks;
using VueCoreBase.Controllers;

namespace Areas.Account.Controllers
{
    public class AccountController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager ,IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signinManager = signInManager;
        }

        /// <summary>
        /// Register a User using ASP.Net Identity.
        /// Creates a user with a username which is an email and a password IAW password requirements policy.
        /// 
        /// 1. Checks the user doesn't already exist, if so return bad request
        /// 2. create user
        /// 3. Adds the Customer Role, if fails delete the user and return bad request
        /// 4. sends email verification
        /// 5. return Ok.
        /// 
        /// </summary>
        /// <param name="model">RegisterViewModel</param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {

            if (!ModelState.IsValid)
                throw new ApiException(ExceptionsTypes.RegistrationError, ModelState, model.Validations);

            ApplicationUser user = null;

            //some type of Database Error
            try
            {
                user = await _userManager.FindByEmailAsync(model.Email);

            }catch(Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }
                
            if (user != null)
                throw new ApiException(ExceptionsTypes.RegistrationError, ExceptionErrors.AccountErrors.UserExists);

            user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            IdentityResult registerResult = null;

            try
            {
                registerResult = await _userManager.CreateAsync(user, model.Password);

            }catch(Exception ex)
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

                var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code.TokenEncode(), Request.Scheme, Request.Host.Value);

                await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl, user.UserName);
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.EmailError, ex);
            }


            return Ok($"New User ({model.Email}) successfully created.");
        }
        /// <summary>
        /// This method is called from an email resulting from the Registration Action
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        // to be idempotent it should be a put, but this is a link coming from an email so it needs to be a get
        // even though it will cause a change in the database
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string id, string code)
        {
            var controller = "Home";

            if (id == null || code == null)
                return RedirectToAction(nameof(HomeController.Index), controller);

            string action = nameof(HomeController.Error);

            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    throw new ApiException(ExceptionsTypes.RegistrationError, $"Unable to load user with ID '{id}'.");

                var result = await _userManager.ConfirmEmailAsync(user, code);

                if (!result.Succeeded)
                    throw new ApiException(ExceptionsTypes.LoginError, result.Errors);

                await _signinManager.SignInAsync(user, true);

                action = nameof(HomeController.Index);

            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.LoginError, ex);
            }

            return RedirectToAction(action, controller);
        }


        /// <summary>
        /// Creates an email for the user to go to a link to reset their password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ExceptionsTypes.ForgotPassword, ModelState, model.Validations);

            ApplicationUser user = null;

            try
            {
                user = await _userManager.FindByEmailAsync(model.Email);

            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }

           
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToAction(nameof(HomeController.Index), "Home");

            try
            {
                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code.TokenEncode(), Request.Scheme, Request.Host.Value);

                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                       $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
            }catch(Exception ex)
            {
                throw new ApiException(ExceptionsTypes.EmailError, ex);
            }
            
            return Ok();
            
        }

        /// <summary>
        /// A post from the application's password reset form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ExceptionsTypes.ForgotPassword, ModelState, model.Validations);

            ApplicationUser user = null;

            try
            {
                user = await _userManager.FindByEmailAsync(model.Email);
            }
            catch(Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }


            if (user == null)
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(HomeController.Index), "Home");


            try
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

                if (result.Succeeded)
                    return Ok();

            }catch(Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }
            

            // Don't reveal error
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// When a user changes their email send verification email so that they confirm that changed mail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> SendVerificationEmail([FromBody] Guid id)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ExceptionsTypes.ForgotPassword, ModelState);


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
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            try
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code.TokenEncode(), Request.Scheme, Request.Host.Value);

                var email = user.Email;

                await _emailSender.SendEmailConfirmationAsync(email, callbackUrl, user.UserName);
            }
            catch(Exception ex)
            {
                throw new ApiException(ExceptionsTypes.EmailError, ex);
            }

           
            // handle in the client
            return Ok();
        }


        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ExceptionsTypes.ChangePassword, ModelState, model.Validations);

            ApplicationUser user = null;

            try
            {
                user = await _userManager.GetUserAsync(User);
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }

            if (user == null)
                throw new ApiException(ExceptionsTypes.ChangePassword, $"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            IdentityResult changePasswordResult = null;
            try
            {
                changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            }catch(Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }


            if (!changePasswordResult.Succeeded)
                throw new ApiException(ExceptionsTypes.ChangePassword, changePasswordResult.Errors);
           

            return Ok("Your password has been changed.");
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ExceptionsTypes.SetPassword, ModelState, model.Validations);

            ApplicationUser user = null;

            try
            {
                user = await _userManager.GetUserAsync(User);
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }


            if (user == null)
            {
                throw new ApiException(ExceptionsTypes.SetPassword, $"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            IdentityResult addPasswordResult = null;

            try
            {
                addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);

            }catch(Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }

            
            if (!addPasswordResult.Succeeded)
                throw new ApiException(ExceptionsTypes.SetPassword, addPasswordResult.Errors);

            return Ok("Your password has been set.");
        }

        [HttpPost("[action]/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserDetailsViewModel model)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ExceptionsTypes.ForgotPassword, ModelState, model.Validations);


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
                throw new ApiException(ExceptionsTypes.UserAccountError, $"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var email = user.Email;

            if (model.Email != email)
            {
                IdentityResult setEmailResult = null;
                try
                {
                    setEmailResult = await _userManager.SetEmailAsync(user, model.Email);

                }catch(Exception ex)
                {
                    throw new ApiException(ExceptionsTypes.DatabaseError, ex);
                }
                

                if (!setEmailResult.Succeeded)
                    throw new ApiException(ExceptionsTypes.UserAccountError, setEmailResult.Errors);
            }

            var phoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                IdentityResult setPhoneResult = null;
                try
                {
                    setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);

                }catch(Exception ex)
                {
                    throw new ApiException(ExceptionsTypes.DatabaseError, ex);
                }
                
                if (!setPhoneResult.Succeeded)
                    throw new ApiException(ExceptionsTypes.UserAccountError, setPhoneResult.Errors);
            }

            return Ok("Your profile has been updated");
        }


    }
}
