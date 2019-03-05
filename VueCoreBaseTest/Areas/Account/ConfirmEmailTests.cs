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
    public class ConfirmEmailTests
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
        public async Task ConfirmEmailPass()
        {
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);

            moqAspIdentity.mockUserManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            moqAspIdentity.mockSignInManager.Setup(x => x.SignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>(), null)).Returns(Task.CompletedTask);

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);
            
            // operation
            try
            {
                Dictionary<string, string> result = await model.ConfirmEmailAsync(moqAspIdentity.mockSignInManager.Object, "122", "123");
                // assert
                Assert.AreEqual(result["action"], "Index");
                Assert.AreEqual(result["controller"], "Home");
            }
            catch
            {
                Assert.Fail("Supposed to pass");
            }

        }

        [TestMethod]
        public async Task ConfirmEmailFailIdNull()
        {

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                Dictionary<string, string> result = await model.ConfirmEmailAsync(moqAspIdentity.mockSignInManager.Object, null, "123");
                // assert
                Assert.AreEqual(result["action"], "Index");
                Assert.AreEqual(result["controller"], "Home");
            }
            catch
            {
                Assert.Fail("Not Supposed to expose Error");
            }
        }

        [TestMethod]
        public async Task ConfirmEmailFailCodeNull()
        {

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                Dictionary<string, string> result = await model.ConfirmEmailAsync(moqAspIdentity.mockSignInManager.Object, "123", null);
                // assert
                Assert.AreEqual(result["action"], "Index");
                Assert.AreEqual(result["controller"], "Home");
            }
            catch
            {
                Assert.Fail("Not Supposed to expose Error");
            }
        }

        [TestMethod]
        public async Task ConfirmEmailFailUserNotFound()
        {
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            moqAspIdentity.mockUserManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            moqAspIdentity.mockSignInManager.Setup(x => x.SignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>(), null)).Returns(Task.CompletedTask);


            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                Dictionary<string, string> result = await model.ConfirmEmailAsync(moqAspIdentity.mockSignInManager.Object, "123", "123");
                // assert
                Assert.AreEqual(result["action"], "Index");
                Assert.AreEqual(result["controller"], "Home");
            }
            catch
            {
                Assert.Fail("Not Supposed to expose Error");
            }
        }

        [TestMethod]
        public async Task ConfirmEmailFailConfirmEmailError()
        {
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);

            moqAspIdentity.mockUserManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(identityError));

            moqAspIdentity.mockSignInManager.Setup(x => x.SignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>(), null)).Returns(Task.FromException(new Exception()));


            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                Dictionary<string, string> result = await model.ConfirmEmailAsync(moqAspIdentity.mockSignInManager.Object, "123", "123");
                // assert
                Assert.Fail("Supposed to throw exception");
            }
            catch(Exception ex)
            {
                if (ex is ApiException)
                {
                    Assert.AreEqual(((ApiException)ex).ExceptionType, ExceptionsTypes.LoginError);
                }
                else
                {
                    Assert.Fail("Wrong Type of exception thrown.");
                }
            }
        }


        [TestMethod]
        public async Task ConfirmEmailFailSignInException()
        {
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);

            moqAspIdentity.mockUserManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            moqAspIdentity.mockSignInManager.Setup(x => x.SignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>(), null)).Returns(Task.FromException(new Exception()));


            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                Dictionary<string, string> result = await model.ConfirmEmailAsync(moqAspIdentity.mockSignInManager.Object, "123", "123");
                // assert
                Assert.Fail("Supposed to throw exception");
            }
            catch (Exception ex)
            {
                if (ex is ApiException)
                {
                    Assert.AreEqual(((ApiException)ex).ExceptionType, ExceptionsTypes.LoginError);
                }
                else
                {
                    Assert.Fail("Wrong Type of exception thrown.");
                }
            }
        }


    }

}
