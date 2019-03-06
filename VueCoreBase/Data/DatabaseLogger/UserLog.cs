using System;
using System.Collections.Generic;

namespace Data.DatabaseLogger
{
    public partial class UserLog
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string TokenType { get; set; }
    }
}
