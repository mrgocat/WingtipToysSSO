﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.POCOS
{
    public class UserHistoryPoco
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime Updated { get; set; }
        public UserPoco SnapShot { get; set; }
        public string UpdateUserId { get; set; }
        public string UpdateReason { get; set; }
    }
}
