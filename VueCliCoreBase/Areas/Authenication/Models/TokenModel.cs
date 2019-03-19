using ASPIdentity.Data;
using Controllers.Exceptions;
using Data.DatabaseLogger;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Base;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Areas.Authenication.Models
{
    public class TokenModel: ModelBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private ASPIdentityContext _mDb { get; set; }
        private DatabaseLoggerContext _lDb { get; set; }

        private string NewRefreshTokenValue => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        public TokenModel(UserManager<ApplicationUser> userManager, IConfiguration configuration, ASPIdentityContext db, DatabaseLoggerContext lDb)
        {
            _configuration = configuration;
            _userManager = userManager;
            _mDb = db;
            _lDb = lDb;
        }

        public async Task<TokenViewModel> GetToken(SignInManager<ApplicationUser> _signInManager, LoginViewModel viewModel, ModelStateDictionary modelState)
        {
            this.ValidateModelState(modelState, viewModel);

            ApplicationUser user = null;

            // database error
            try
            {
                user = await _userManager.FindByEmailAsync(viewModel.Email);
            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.DatabaseError, ex);
            }



            if (user == null)
                throw new ApiException(ExceptionsTypes.LoginError, AuthenicationConstants.BadRequestMessages.InvalidCredentials);

            if (await _userManager.IsLockedOutAsync(user))
                throw new ApiException(ExceptionsTypes.LoginError, AuthenicationConstants.BadRequestMessages.LockedOut);

            try
            {
                var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, true, true);
                if (!result.Succeeded)
                    throw new ApiException(ExceptionsTypes.LoginError, AuthenicationConstants.BadRequestMessages.InvalidCredentials);

                var rt = await _mDb.UserTokens.SingleOrDefaultAsync(a =>
                                                a.LoginProvider == AuthenicationConstants.LoginProvider &&
                                                a.Name == AuthenicationConstants.TokenName &&
                                                a.UserId == user.Id
                                                );

                if (rt == null)
                {
                    rt = CreateRefreshToken(user.Id);
                    _mDb.UserTokens.Add(rt);
                }
                else
                {
                    rt.Value = NewRefreshTokenValue;
                }

                // persist changes in the DB
                await _mDb.SaveChangesAsync();

                var token = await GenerateToken(user, rt.Value);


                await _lDb.UserLog.AddAsync(new UserLog {
                    DateTimeStamp = DateTime.UtcNow,
                    TokenType = "Token",
                    UserName = user.UserName

                });

                await _lDb.SaveChangesAsync();

                return token;

            }
            catch (Exception ex)
            {
                throw new ApiException(ExceptionsTypes.LoginError, ex);
            }

        }

        public async Task<TokenViewModel> Refreshtoken(RefreshTokenViewModel viewModel, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                throw new ApiException(ExceptionsTypes.LoginError, modelState);

                var rt = await _mDb.UserTokens.SingleOrDefaultAsync(x =>
                                                                x.UserId == viewModel.UserId &&
                                                                x.Value == viewModel.RefreshToken &&
                                                                x.LoginProvider == AuthenicationConstants.LoginProvider &&
                                                                x.Name == AuthenicationConstants.TokenName);

            if (rt == null)
                // refresh token not found or invalid (or invalid clientId)
                throw new ApiException(ExceptionsTypes.LoginError, "Refresh Token Error");

            // check if there's an user with the refresh token's userId
            var user = await _userManager.FindByIdAsync(rt.UserId.ToString());

            if (user == null)
                // UserId not found or invalid
                throw new ApiException(ExceptionsTypes.LoginError, "Refresh Token Error");

            rt.Value = NewRefreshTokenValue;

            // persist changes in the DB
            await _mDb.SaveChangesAsync();

            var token = await GenerateToken(user, rt.Value);

            await _lDb.UserLog.AddAsync(new UserLog
            {
                DateTimeStamp = DateTime.UtcNow,
                TokenType = "Refresh",
                UserName = user.UserName

            });

            await _lDb.SaveChangesAsync();

            return token;
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
