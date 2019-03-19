using Models.VeeValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers.Exceptions
{
    public class ApiError
    {

        public ApiError(ApiException ex)
        {
            this.isError = true;
            this.errors = ex.Errors;
            this.properties = ex.Validations;
        }

        public ApiError(string message)
        {
            this.isError = true;
            errors = new List<string> { message };
        }
        public List<string> errors { get; set; }
        public bool isError { get; set; }
        public Dictionary<string, ClassProperty> properties { get; set; }

        

    }
}
