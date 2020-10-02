using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WingtipSSO.BusinessLogicLayer.Identity
{
    public class JwtOptions
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public int JwtExpireDays { get; set; }
    }
}
