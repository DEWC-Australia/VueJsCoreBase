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
    public class SendVerificationEmailTests
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
        public async Task SendVerificationEmailPass()
        {
            applicationUser.Email = "test@email.com";
            applicationUser.UserName = applicationUser.Email;
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);
            moqAspIdentity.mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("token");
 
            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);
            
            // operation
            try
            {
                string result = await model.SendVerificationEmail(emailSender, Guid.NewGuid(), moqAspIdentity.mockModelState.Object, "http", "www.test.com");
                // assert
                Assert.AreEqual(result, AccountConstants.SendVerificationEmailMsg);
            }
            catch
            {
                Assert.Fail("Supposed to pass");
            }
        }

        [TestMethod]
        public async Task SendVerificationEmailFailUserNotExist()
        {
            applicationUser.Email = "test@email.com";
            applicationUser.UserName = applicationUser.Email;
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.SendVerificationEmail(emailSender, Guid.NewGuid(), moqAspIdentity.mockModelState.Object, "http", "www.test.com");
                // assert
                Assert.Fail("Should throw exception.");
            }
            catch(Exception ex)
            {
                if (ex is ApiException)
                {
                    Assert.AreEqual(((ApiException)ex).ExceptionType, ExceptionsTypes.RegistrationError);
                }
                else
                {
                    Assert.Fail("Wrong Type of exception thrown.");
                }
            }

        }

        [TestMethod]
        public async Task SendVerificationEmailFailEmailError()
        {
            applicationUser.Email = null;
            applicationUser.UserName = applicationUser.Email;
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);
            moqAspIdentity.mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("token");
            moqAspIdentity.mockUserManager.Setup(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.SendVerificationEmail(emailSender, Guid.NewGuid(), moqAspIdentity.mockModelState.Object, "http", "www.test.com");
                // assert
                Assert.Fail("Should throw exception.");
            }
            catch (Exception ex)
            {
                if (ex is ApiException)
                {
                    Assert.AreEqual(((ApiException)ex).ExceptionType, ExceptionsTypes.EmailError); 
                }
                else
                {
                    Assert.Fail("Wrong Type of exception thrown.");
                }
            }

        }
    }

}
