using System;
using System.Collections.Generic;

namespace Data.DatabaseLogger
{
    public partial class RequestLog
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public int? StatusCode { get; set; }
        public string UserName { get; set; }
    }
}
