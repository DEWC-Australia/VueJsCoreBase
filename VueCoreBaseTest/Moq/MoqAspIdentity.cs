using ASPIdentity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Services.Email;
using System;
using System.Security.Claims;
using System.Threading;


namespace VueCoreBaseTest.Moq
{
    class MoqAspIdentity : MoqBase
    {
        private Mock<IUserStore<ApplicationUser>> userStore = new Mock<IUserStore<ApplicationUser>>();
        public Mock<ASPIdentityContext> mockAspIdentity = new Mock<ASPIdentityContext>();
        public Mock<UserManager<ApplicationUser>> mockUserManager { get; set; }
        public Mock<SignInManager<ApplicationUser>> mockSignInManager = new Mock<SignInManager<ApplicationUser>>();
        public Mock<IEmailSender> mockEmailSender = new Mock<IEmailSender>();
        public Mock<ModelStateDictionary> mockModelState = new Mock<ModelStateDictionary>();

        public Mock<ClaimsPrincipal> mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        public MoqAspIdentity()
        {
            
            CoreFunctions();
        }

        public Mock<ASPIdentityContext> GetAspIdentity()
        {
            return mockAspIdentity;
        }

        protected void CoreFunctions()
        {
           

            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStore.Object,
              new Mock<IOptions<IdentityOptions>>().Object,
              new Mock<IPasswordHasher<ApplicationUser>>().Object,
              new IUserValidator<ApplicationUser>[0],
              new IPasswordValidator<ApplicationUser>[0],
              new Mock<ILookupNormalizer>().Object,
              new Mock<IdentityErrorDescriber>().Object,
              new Mock<IServiceProvider>().Object,
              null);

            var _contextAccessor = new Mock<IHttpContextAccessor>();
            var _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager.Object,
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null);
                
            SaveChanges();
        }

        protected void SaveChanges()
        {
            mockAspIdentity.Setup(x => x.SaveChanges()).Verifiable();

            mockAspIdentity.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Verifiable();
        }

    }
}
