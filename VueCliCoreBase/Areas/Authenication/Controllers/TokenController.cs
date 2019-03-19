using ASPIdentity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Areas.Authenication.Models;
using System;
using System.Threading.Tasks;
using Controllers.Base;
using Data.DatabaseLogger;

namespace Areas.Authenication.Controllers
{
    public class TokenController : ApiBaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
       
        private TokenModel _model { get; set; }

        private string NewRefreshTokenValue => Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        public TokenController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration, ASPIdentityContext db, DatabaseLoggerContext lDb)
        {
            _signInManager = signInManager;
            
            _model = new TokenModel(userManager, configuration, db, lDb);
        }

        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] LoginViewModel viewModel)
        {
            var token = await _model.GetToken(_signInManager, viewModel, ModelState);

            return Ok(token);

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refreshtoken([FromBody] RefreshTokenViewModel viewModel)
        {

            var token = await _model.Refreshtoken(viewModel, ModelState);

            return Ok(token);
            
        }
        
    }
}
