using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Controllers.Base
{

    public abstract class BaseController : Controller
    {
        protected JsonSerializerSettings JsonSettings { get; private set; }
        public BaseController()
        {
            // Instantiate a single JsonSerializerSettings object
            // that can be reused multiple times.
            JsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        }
    }
}
