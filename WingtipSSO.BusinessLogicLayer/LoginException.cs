using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.BusinessLogicLayer
{
    public class LoginException : Exception
    {
        public LoginException(string message) : base(message)
        {
        }
    }
}
