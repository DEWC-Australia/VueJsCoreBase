using Areas.Account.Models;
using ASPIdentity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Email;
using System;
using System.Threading.Tasks;
using Controllers.Base;


namespace Areas.Account.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IEmailSender _emailSender;
        private AccountModel _model { get; set; }

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager ,IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _signinManager = signInManager;
            _model = new AccountModel(userManager);
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
        public async Task<IActionResult> Register([FromBody] RegisterViewModel viewModel)
        {
            var result = await _model.Register(_emailSender, viewModel, ModelState, Request.Scheme, Request.Host.Value);

            return Ok(result);
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
            var result = await _model.ConfirmEmailAsync(_signinManager, id, code);

            return RedirectToAction(result["action"], result["controller"]);
        }


        /// <summary>
        /// Creates an email for the user to go to a link to reset their password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel viewModel)
        {
            var result = await _model.ForgotPassword(_emailSender, viewModel, ModelState, Request.Scheme, Request.Host.Value);

            return Ok(result);
            
        }

        /// <summary>
        /// A post from the application's password reset form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel viewModel)
        {
            var result = await _model.ResetPassword(viewModel, ModelState);

            // client should provide a redirect link to login
            return Ok(result);
            
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
            var result = await _model.SendVerificationEmail(_emailSender,id, ModelState, Request.Scheme, Request.Host.Value);
            // handle in the client
            return Ok(result);
        }


        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel viewModel)
        {
            var result = await _model.ChangePassword(viewModel, ModelState, User);

            return Ok(result);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordViewModel viewModel)
        {
            var result = await _model.SetPassword(viewModel, ModelState, User);

            return Ok(result);
        }

        [HttpPut("[action]/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserDetailsViewModel viewModel)
        {
            var result = await _model.Update(id, viewModel, ModelState);
            return Ok(result);
        }


    }
}
