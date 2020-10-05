using System;
using System.Collections.Generic;
using System.Text;
using WingtipSSO.POCOS;

namespace WingtipSSO.BusinessLogicLayer
{
    public interface IUserService
    {
        string Create(UserPoco poco);
        void UpdatePasswrod(string UserId, string OldPassword, string NewPassword);
        void Update(UserPoco poco);
        void Patch(string userId, string key, string value);
        UserPoco Get(string userId);
        IList<UserPoco> Get();
        UserPoco Authenticate(string userId, string password, string loginIP = null);
        public bool CheckIdExists(string userId);
    }
}
