using Microsoft.AspNetCore.Mvc;


namespace Controllers.Base
{
    [Route("api/[controller]")]
    public abstract class ApiBaseController : BaseController
    {
        public ApiBaseController() : base()
        {
        }
    }
}
