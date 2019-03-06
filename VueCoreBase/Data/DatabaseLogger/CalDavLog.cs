using System;
using System.Collections.Generic;

namespace Data.DatabaseLogger
{
    public partial class CalDavLog
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int? StatusCode { get; set; }
        public string ResponseContentType { get; set; }
    }
}
