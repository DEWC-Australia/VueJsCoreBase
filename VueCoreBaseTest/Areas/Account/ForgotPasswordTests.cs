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
    public class ForgotPasswordTests
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
        public async Task ForgotPasswordPass()
        {
            ForgotPasswordViewModel viewModel = new ForgotPasswordViewModel
            {
                Email = "joe.bloggs@company.com"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);

            moqAspIdentity.mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            moqAspIdentity.mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            moqAspIdentity.mockUserManager.Setup(x => x.IsEmailConfirmedAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(true);

            moqAspIdentity.mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("token");
           
            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);
            
            // operation
            try
            {
                string result = await model.ForgotPassword(emailSender, viewModel, moqAspIdentity.mockModelState.Object, "http", "www.test.com");
                // assert
                Assert.AreEqual(result, AccountConstants.ForgotPasswordMsg);
            }
            catch
            {
                Assert.Fail("Supposed to pass");
            }

        }

        [TestMethod]
        public async Task ForgotPasswordFailUserDoesntExists()
        {
            ForgotPasswordViewModel viewModel = new ForgotPasswordViewModel
            {
                Email = "joe.bloggs@company.com"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.ForgotPassword(emailSender, viewModel, moqAspIdentity.mockModelState.Object, "http", "www.test.com");
                // assert
                Assert.AreEqual(result, AccountConstants.ForgotPasswordMsg);
            }
            catch
            {
                Assert.Fail("Supposed to pass");
            }
        }
        
        [TestMethod]
        public async Task ForgotPasswordFailEmailNotConfirmed()
        {
            ForgotPasswordViewModel viewModel = new ForgotPasswordViewModel
            {
                Email = "joe.bloggs@company.com"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);
            moqAspIdentity.mockUserManager.Setup(x => x.IsEmailConfirmedAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(false);
            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.ForgotPassword(emailSender, viewModel, moqAspIdentity.mockModelState.Object, "http", "www.test.com");
                // assert
                Assert.AreEqual(result, AccountConstants.ForgotPasswordMsg);
            }
            catch
            {
                Assert.Fail("Supposed to pass");
            }
        }
    }

}
