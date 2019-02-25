using ASPIdentity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueCoreBase.Data.VueCoreBase;

namespace VueCoreBase.Controllers
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
