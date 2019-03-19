using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.VeeValidation
{
    public class ClassProperty
    {
        public string displayName { get; set; }
        public string value { get; set; }

        public Dictionary<string, dynamic> validations { get; set; }
    }
}
