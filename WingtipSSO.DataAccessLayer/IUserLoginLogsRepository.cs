using System;
using System.Collections.Generic;
using WingtipSSO.POCOS;

namespace WingtipSSO.DataAccessLayer
{
    public interface IUserLoginLogsRepository
    {
        public string Create(UserLoginLogPoco poco);
    }
}
