using System;
using System.Collections.Generic;
using WingtipSSO.POCOS;

namespace WingtipSSO.DataAccessLayer
{
    public interface IUserWrongPasswordRepository
    {
        public string Create(UserWrongPasswordPoco poco);
        public UserWrongPasswordPoco Find(string userId);
        public void IncreaseFailedCount(string userId);
        public void Delete(string userId);
    }
}
