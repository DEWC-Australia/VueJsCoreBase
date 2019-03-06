using Microsoft.AspNetCore.Authorization;
using Data.VueCoreBase;

namespace Controllers.Base
{
    [Authorize]
    public abstract class ApiAuthBaseController : ApiBaseController
    {

        public ApiAuthBaseController() : base()
        {

        }
    }
}
