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
        public bool Update(UserPoco poco);
        public bool Patch<T>(string userId, string key, T value);
        public bool UpdatePassword(string id, string passwordHash);
        public bool LockUser(string id);
        public Boolean CheckIdExists(string userId);
    }
}
