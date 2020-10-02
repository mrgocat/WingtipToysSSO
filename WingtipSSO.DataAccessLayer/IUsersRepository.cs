using System;
using System.Collections.Generic;
using WingtipSSO.POCOS;

namespace WingtipSSO.DataAccessLayer
{
    public interface IUsersRepository
    {
        public UserPoco Create(UserPoco poco);
        public IList<UserPoco> Read();
        public UserPoco Find(string id);
    }
}
