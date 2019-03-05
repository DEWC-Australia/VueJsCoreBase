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
    public class UpdateTests
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
        public async Task UpdatePass()
        {
            UserDetailsViewModel viewModel = new UserDetailsViewModel
            {
                Email = "test@email.com",
                PhoneNumber = "0403478235"
            };
            applicationUser.Email = "test@email.com";
            applicationUser.PhoneNumber = "0403478235";
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);
            moqAspIdentity.mockUserManager.Setup(x => x.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            moqAspIdentity.mockUserManager.Setup(x => x.SetPhoneNumberAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.Update(Guid.NewGuid(), viewModel, moqAspIdentity.mockModelState.Object);
                // assert
                Assert.AreEqual(result, AccountConstants.UpdateMsg);
            }
            catch
            {
                Assert.Fail("Supposed to pass");
            }
        }

        [TestMethod]
        public async Task UpdatePassChangeEmail()
        {
            UserDetailsViewModel viewModel = new UserDetailsViewModel
            {
                Email = "test@email.com",
                PhoneNumber = "0403478235"
            };
            applicationUser.Email = "test@email.co";
            applicationUser.PhoneNumber = "0403478235";
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);
            moqAspIdentity.mockUserManager.Setup(x => x.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            moqAspIdentity.mockUserManager.Setup(x => x.SetPhoneNumberAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.Update(Guid.NewGuid(), viewModel, moqAspIdentity.mockModelState.Object);
                // assert
                Assert.AreEqual(result, AccountConstants.UpdateMsg);
            }
            catch (Exception ex)
            {
                Assert.Fail("Supposed to pass");
            }
        }

        [TestMethod]
        public async Task UpdatePassChangePhone()
        {
            UserDetailsViewModel viewModel = new UserDetailsViewModel
            {
                Email = "test@email.com",
                PhoneNumber = "0403478235"
            };
            applicationUser.Email = "test@email.com";
            applicationUser.PhoneNumber = "040347823";
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);
            moqAspIdentity.mockUserManager.Setup(x => x.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            moqAspIdentity.mockUserManager.Setup(x => x.SetPhoneNumberAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.Update(Guid.NewGuid(), viewModel, moqAspIdentity.mockModelState.Object);
                // assert
                Assert.AreEqual(result, AccountConstants.UpdateMsg);
            }
            catch (Exception ex)
            {
                Assert.Fail("Supposed to pass");
            }
        }

        [TestMethod]
        public async Task UpdateFailNoUser()
        {
            UserDetailsViewModel viewModel = new UserDetailsViewModel
            {
                Email = "test@email.com",
                PhoneNumber = "0403478235"
            };
            applicationUser.Email = "test@email.com";
            applicationUser.PhoneNumber = "0403478235";
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);
            
            // operation
            try
            {
                string result = await model.Update(Guid.NewGuid(), viewModel, moqAspIdentity.mockModelState.Object);
                // assert
                Assert.Fail("Should throw exception.");
            }
            catch(Exception ex)
            {
                if (ex is ApiException)
                {
                    Assert.AreEqual(((ApiException)ex).ExceptionType, ExceptionsTypes.UserAccountError);
                }
                else
                {
                    Assert.Fail("Wrong Type of exception thrown.");
                }
            }
        }

        [TestMethod]
        public async Task UpdateFailEmailError()
        {
            UserDetailsViewModel viewModel = new UserDetailsViewModel
            {
                Email = "test@email.com",
                PhoneNumber = "0403478235"
            };
            applicationUser.Email = "test@email.co";
            applicationUser.PhoneNumber = "0403478235";
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);
            moqAspIdentity.mockUserManager.Setup(x => x.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(this.identityError));
            moqAspIdentity.mockUserManager.Setup(x => x.SetPhoneNumberAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.Update(Guid.NewGuid(), viewModel, moqAspIdentity.mockModelState.Object);
                // assert
                Assert.Fail("Should throw exception.");
            }
            catch (Exception ex)
            {
                if (ex is ApiException)
                {
                    Assert.AreEqual(((ApiException)ex).ExceptionType, ExceptionsTypes.UserAccountError);
                }
                else
                {
                    Assert.Fail("Wrong Type of exception thrown.");
                }
            }
        }

        [TestMethod]
        public async Task UpdateFailPhoneError()
        {
            UserDetailsViewModel viewModel = new UserDetailsViewModel
            {
                Email = "test@email.com",
                PhoneNumber = "0403478235"
            };
            applicationUser.Email = "test@email.com";
            applicationUser.PhoneNumber = "040347823";
            moqAspIdentity.mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);
            moqAspIdentity.mockUserManager.Setup(x => x.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            moqAspIdentity.mockUserManager.Setup(x => x.SetPhoneNumberAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(this.identityError));

            // setup
            AccountModel model = new AccountModel(moqAspIdentity.mockUserManager.Object);

            // operation
            try
            {
                string result = await model.Update(Guid.NewGuid(), viewModel, moqAspIdentity.mockModelState.Object);
                // assert
                Assert.Fail("Should throw exception.");
            }
            catch (Exception ex)
            {
                if (ex is ApiException)
                {
                    Assert.AreEqual(((ApiException)ex).ExceptionType, ExceptionsTypes.UserAccountError);
                }
                else
                {
                    Assert.Fail("Wrong Type of exception thrown.");
                }
            }
        }
    }

}
