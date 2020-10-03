using System;
using System.Collections.Generic;
using WingtipSSO.POCOS;

namespace WingtipSSO.DataAccessLayer
{
    public interface IUsersRepository
    {
        public void Create(UserPoco poco);
        public IList<UserPoco> Read();
        public UserPoco Find(string id);
        public void Update(UserPoco poco);
        public void UpdatePassword(string id, string passwordHash);
        public void LockUser(string id);
    }
}
