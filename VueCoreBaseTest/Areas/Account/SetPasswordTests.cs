using Areas.Account.Models;
using ASPIdentity.Data;
using Controllers.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Authorisation;
using Moq;
using Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VueCoreBaseTest.Moq;

namespace VueCoreBaseTest.Areas.Account
{
    [TestClass]
    public class SetPasswordTests
    {
        private MoqAspIdentity moqAspIdentity { get; set; }

        private ApplicationUser applicationUser { get; set; }
        private EmailSender emailSender { get; set; }

        private IdentityError[] identityError = new IdentityError[] { new IdentityError { Code = "testCode", Description = "Test Error" } };


        [TestInitialize()]
        public void Init()
        {
            moqAspIdentity = new MoqAspIdentity();

            emailSender = new EmailSender();

            applicationUser = new ApplicationUser
            { 
                FirstName = "Joe",
                LastName = "Bloggs"
            };
        }

        [TestMethod]
        public async Task SetPasswordPass()
        {
            SetPasswordViewModel viewModel = new SetPasswordViewModel
            {
                ConfirmPassword = "password",
                NewPassword = "password"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(applicationUser);
            moqAspIdentity.mockUserManager.Setup(x => x.AddPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
 
            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);
            
            // operation
            try
            {
                string result = await model.SetPassword(viewModel, moqAspIdentity.mockModelState.Object, moqAspIdentity.mockClaimsPrincipal.Object);
                // assert
                Assert.AreEqual(result, AccountConstants.SetPasswordMsg);
            }
            catch
            {
                Assert.Fail("Supposed to pass");
            }
        }

        [TestMethod]
        public async Task SetPasswordFailNoUser()
        {
            SetPasswordViewModel viewModel = new SetPasswordViewModel
            {
                ConfirmPassword = "password",
                NewPassword = "password"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((ApplicationUser)null);

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.SetPassword(viewModel, moqAspIdentity.mockModelState.Object, moqAspIdentity.mockClaimsPrincipal.Object);
                // assert
                Assert.Fail("Should throw exception.");
            }
            catch (Exception ex)
            {
                if (ex is ApiException)
                {
                    Assert.AreEqual(((ApiException)ex).ExceptionType, ExceptionsTypes.SetPassword);
                }
                else
                {
                    Assert.Fail("Wrong Type of exception thrown.");
                }
            }
        }


        [TestMethod]
        public async Task SetPasswordFail_AddPasswordError()
        {
            SetPasswordViewModel viewModel = new SetPasswordViewModel
            {
                ConfirmPassword = "password",
                NewPassword = "password"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(applicationUser);
            moqAspIdentity.mockUserManager.Setup(x => x.AddPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(this.identityError));

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.SetPassword(viewModel, moqAspIdentity.mockModelState.Object, moqAspIdentity.mockClaimsPrincipal.Object);
                // assert
                Assert.Fail("Should throw exception.");
            }
            catch(Exception ex)
            {
                if (ex is ApiException)
                {
                    Assert.AreEqual(((ApiException)ex).ExceptionType, ExceptionsTypes.SetPassword);
                }
                else
                {
                    Assert.Fail("Wrong Type of exception thrown.");
                }
            }
        }
    }

}
