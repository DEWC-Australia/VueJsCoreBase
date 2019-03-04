using ASPIdentity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Areas.Authenication.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VueCoreBase.Controllers;
using Microsoft.EntityFrameworkCore;
using Controllers.Exceptions;

namespace Areas.Authenication.Controllers
{
    public class TokenController : ApiController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private ASPIdentityContext mDb;

        private string NewRefreshTokenValue => Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        public TokenController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration, ASPIdentityContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            mDb = db;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] LoginViewModel model)
        {

            if (!ModelState.IsValid)
                throw new ApiException(ExceptionsTypes.LoginError, ModelState, model.Validations);

            ApplicationUser user = null;

            // database error
            try
            {
                user = await _userManager.FindByEmailAsync(model.Email);
            }
            catch(Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }

            

            if (user == null)
                throw new ApiException(ExceptionsTypes.LoginError, AuthenicationConstants.BadRequestMessages.InvalidCredentials);

            if (await _userManager.IsLockedOutAsync(user))
                throw new ApiException(ExceptionsTypes.LoginError, AuthenicationConstants.BadRequestMessages.LockedOut);

            try
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
                if (!result.Succeeded)
                    throw new ApiException(ExceptionsTypes.LoginError, AuthenicationConstants.BadRequestMessages.InvalidCredentials);

                var rt = await mDb.UserTokens.SingleOrDefaultAsync(a =>
                                                a.LoginProvider == AuthenicationConstants.LoginProvider &&
                                                a.Name == AuthenicationConstants.TokenName &&
                                                a.UserId == user.Id
                                                );

                if (rt == null)
                {
                    rt = CreateRefreshToken(user.Id);
                    mDb.UserTokens.Add(rt);
                }
                else
                {
                    rt.Value = NewRefreshTokenValue;
                }

                // persist changes in the DB
                await mDb.SaveChangesAsync();

                var token = await GenerateToken(user, rt.Value);

                return Ok(token);

            }
            catch(Exception ex)
            {
                throw new ApiException(ExceptionsTypes.LoginError, ex);
            }
               
            
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refreshtoken([FromBody] RefreshTokenViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var rt = await mDb.UserTokens.SingleOrDefaultAsync(x => 
                                                                x.UserId == model.UserId && 
                                                                x.Value == model.RefreshToken &&
                                                                x.LoginProvider == AuthenicationConstants.LoginProvider &&
                                                                x.Name == AuthenicationConstants.TokenName);
            
            if (rt == null)
                // refresh token not found or invalid (or invalid clientId)
                return BadRequest();

            // check if there's an user with the refresh token's userId
            var user = await _userManager.FindByIdAsync(rt.UserId.ToString());

            if (user == null)
                // UserId not found or invalid
                return BadRequest();

            rt.Value = NewRefreshTokenValue;

            // persist changes in the DB
            await mDb.SaveChangesAsync();

            var token = await GenerateToken(user, rt.Value);
            return Ok(token);
        }

        private async Task<TokenViewModel> GenerateToken(ApplicationUser user, string refreshToken)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Authentication:JwtExpireMins"]));

            var token = new JwtSecurityToken(
              _configuration["Authentication:JwtIssuer"],
              _configuration["Authentication:JwtAudience"],
              claims,
              expires: expires,
              signingCredentials: creds
            );

            return new TokenViewModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                FullName = user.FullName,
                UserId = user.Id,
                Roles = roles

            };
        }

        private IdentityUserToken<Guid> CreateRefreshToken(Guid userId)
        {
            return new IdentityUserToken<Guid>
            {
                LoginProvider = AuthenicationConstants.LoginProvider,
                Name = AuthenicationConstants.TokenName,
                UserId = userId,
                Value = NewRefreshTokenValue
            };
        }

        



    }
}
