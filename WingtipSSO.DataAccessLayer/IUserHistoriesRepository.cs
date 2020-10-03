using System;
using System.Collections.Generic;
using WingtipSSO.POCOS;

namespace WingtipSSO.DataAccessLayer
{
    public interface IUserHistoriesRepository
    {
    //    public string Create(UserHistoryPoco poco);
        public string Create(string userId, string updateUserId, string updateReason);
    }
}
