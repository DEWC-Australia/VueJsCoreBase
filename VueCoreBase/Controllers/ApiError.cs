using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers.Exceptions
{
    public class ApiError
    {
        public string message { get; set; }
        public bool isError { get; set; }

        public ApiError(string message)
        {
            this.message = message;
            isError = true;
        }

    }
}
