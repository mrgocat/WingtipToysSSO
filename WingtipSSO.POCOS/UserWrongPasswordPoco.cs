using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.POCOS
{
    public class UserWrongPasswordPoco
    {
        public string Id { get; set; }
        public int FailedCount { get; set; }
    }
}
