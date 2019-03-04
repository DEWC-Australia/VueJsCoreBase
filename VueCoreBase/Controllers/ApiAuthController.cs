using Microsoft.AspNetCore.Authorization;
using Data.VueCoreBase;

namespace Controllers.Base
{
    [Authorize]
    public abstract class ApiAuthBaseController : ApiBaseController
    {
        protected VueCoreBaseContext _appDb { get; private set; }
        public ApiAuthBaseController(VueCoreBaseContext appContext) : base()
        {
            _appDb = appContext;
        }
    }
}
