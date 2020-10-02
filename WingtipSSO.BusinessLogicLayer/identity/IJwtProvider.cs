using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WingtipSSO.POCOS;

namespace WingtipSSO.BusinessLogicLayer.Identity
{
    public interface IJwtProvider
    {
        public string GenerateJwtToken(UserPoco user);
    }
}
