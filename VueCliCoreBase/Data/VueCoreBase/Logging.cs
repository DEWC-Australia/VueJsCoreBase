using System;
using System.Collections.Generic;

namespace Data.VueCoreBase
{
    public partial class Logging
    {
        public Guid LogId { get; set; }
        public DateTime Created { get; set; }
        public string RequestMethod { get; set; }
        public string RequestPath { get; set; }
        public int ResponseStatusCode { get; set; }
    }
}
