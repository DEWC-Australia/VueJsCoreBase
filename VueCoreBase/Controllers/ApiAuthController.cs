using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPIdentity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VueCoreBase.Data.VueCoreBase;

namespace VueCoreBase.Controllers
{
    [Authorize]
    public abstract class ApiAuthController : ApiController
    {
        protected VueCoreBaseContext _appDb { get; private set; }
        public ApiAuthController(VueCoreBaseContext appContext) : base()
        {
            _appDb = appContext;
        }
    }
}
