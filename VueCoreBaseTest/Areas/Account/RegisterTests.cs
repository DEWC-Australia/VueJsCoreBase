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
    public class RegisterTests
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
        public async Task RegisterPass()
        {
            RegisterViewModel viewModel = new RegisterViewModel
            {
                ConfirmPassword = "password",
                Password = "password",
                Email = "joe.bloggs@company.com",
                FirstName = "Joe",
                LastName = "Bloggs"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            moqAspIdentity.mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            moqAspIdentity.mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            moqAspIdentity.mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("token");
           
            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);
            
            // operation
            try
            {
                string result = await model.Register(emailSender, viewModel, moqAspIdentity.mockModelState.Object, "http", "www.test.com");
                // assert
                Assert.AreEqual(result, $"{AccountConstants.RegisterResponseMsgStart}{viewModel.Email}{AccountConstants.RegisterResponseMsgEnd}");
            }
            catch
            {
                Assert.Fail("Supposed to pass");
            }

        }

        [TestMethod]
        public async Task RegisterFailUserExists()
        {
            RegisterViewModel viewModel = new RegisterViewModel
            {
                ConfirmPassword = "password",
                Password = "password",
                Email = "joe.bloggs@company.com",
                FirstName = "Joe",
                LastName = "Bloggs"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);


            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.Register(emailSender, viewModel, moqAspIdentity.mockModelState.Object, "http", "www.test.com");
                // assert
                Assert.Fail("Should throw exception.");
            }
            catch(Exception ex)
            {
                if(ex is ApiException)
                {
                    Assert.AreEqual(((ApiException)ex).Errors.First(), ExceptionErrors.AccountErrors.UserExists);
                }
                else
                {
                    Assert.Fail("Wrong Type of exception thrown.");
                }
            }
        }

        [TestMethod]
        public async Task RegisterFailCreateUser()
        {
            RegisterViewModel viewModel = new RegisterViewModel
            {
                ConfirmPassword = "password",
                Password = "password",
                Email = "joe.bloggs@company.com",
                FirstName = "Joe",
                LastName = "Bloggs"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            moqAspIdentity.mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(identityError));

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.Register(emailSender, viewModel, moqAspIdentity.mockModelState.Object, "http", "www.test.com");
                // assert
                Assert.Fail("Should throw exception.");
            }
            catch (Exception ex)
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
        public async Task RegisterFailAddRole()
        {
            RegisterViewModel viewModel = new RegisterViewModel
            {
                ConfirmPassword = "password",
                Password = "password",
                Email = "joe.bloggs@company.com",
                FirstName = "Joe",
                LastName = "Bloggs"
            };

            moqAspIdentity.mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            moqAspIdentity.mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            moqAspIdentity.mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(identityError));

            moqAspIdentity.mockUserManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);


            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.Register(emailSender, viewModel, moqAspIdentity.mockModelState.Object, "http", "www.test.com");
                // assert
                Assert.Fail("Should throw exception.");
            }
            catch (Exception ex)
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

        
    }

}
