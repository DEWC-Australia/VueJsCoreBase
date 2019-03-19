using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.VeeValidation
{
    public class PropertyValidation
    {
        public string Name { get; set; }
        public string ErrorMessage { get; set; }
        public Dictionary<string, dynamic> VeeValidation { get; set; }

    }

}
