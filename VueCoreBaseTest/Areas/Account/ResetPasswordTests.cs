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
using System.Threading.Tasks;
using VueCoreBaseTest.Moq;

namespace VueCoreBaseTest.Areas.Account
{
    [TestClass]
    public class ResetPasswordTests
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
        public async Task ResetPasswordPass()
        {
            ResetPasswordViewModel viewModel = new ResetPasswordViewModel
            {
                Code = "123",
                ConfirmPassword = "Password",
                Password = "Password"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);

            moqAspIdentity.mockUserManager.Setup(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
         
            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);
            
            // operation
            try
            {
                string result = await model.ResetPassword(viewModel, moqAspIdentity.mockModelState.Object);
                // assert
                Assert.AreEqual(result, AccountConstants.ResetPasswordMsg);
            }
            catch
            {
                Assert.Fail("Supposed to pass");
            }

        }
        [TestMethod]
        public async Task ResetPasswordFailUserNotExist()
        {
            ResetPasswordViewModel viewModel = new ResetPasswordViewModel
            {
                Code = "123",
                ConfirmPassword = "Password",
                Password = "Password"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            moqAspIdentity.mockUserManager.Setup(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.ResetPassword(viewModel, moqAspIdentity.mockModelState.Object);
                // assert
                Assert.AreEqual(result, AccountConstants.ResetPasswordMsg);
            }
            catch
            {
                Assert.Fail("Supposed to pass");
            }

        }

        [TestMethod]
        public async Task ResetPasswordFailResetError()
        {
            ResetPasswordViewModel viewModel = new ResetPasswordViewModel
            {
                Code = "123",
                ConfirmPassword = "Password",
                Password = "Password"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);

            moqAspIdentity.mockUserManager.Setup(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(identityError));

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.ResetPassword(viewModel, moqAspIdentity.mockModelState.Object);
                // assert
                Assert.AreEqual(result, AccountConstants.ResetPasswordMsg);
            }
            catch
            {
                Assert.Fail("Supposed to pass");
            }

        }
    }

}
