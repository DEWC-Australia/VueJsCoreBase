using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPIdentity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueCoreBase.Data.VueCoreBase;

namespace VueCoreBase.Controllers
{
    [Route("api/[controller]")]
    public abstract class ApiController : BaseController
    {
        public ApiController() : base()
        {
        }
    }
}
