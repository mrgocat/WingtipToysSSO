using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.POCOS
{
    public class UserLoginLogPoco
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string LoginIP { get; set; }
        public DateTime LoginDate { get; set; }
        public bool Failed { get; set; }
        public string FailedReason { get; set; }
    }
}
