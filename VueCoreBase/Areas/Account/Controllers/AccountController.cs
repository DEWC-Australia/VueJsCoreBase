using Areas.Account.Models;
using ASPIdentity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Areas.Account.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
                return BadRequest("A user with that e-mail address already exists!");

            user = new ApplicationUser
            {
                Email = model.Email,
                EmailConfirmed = true,
                UserName = model.Email,
                LockoutEnabled = true
            };

            var registerResult = await _userManager.CreateAsync(user, model.Password);

            if (!registerResult.Succeeded)
                return BadRequest(registerResult.Errors);

            await _userManager.AddToRoleAsync(user, "Customer");

            return Ok();
        }
    }
}
