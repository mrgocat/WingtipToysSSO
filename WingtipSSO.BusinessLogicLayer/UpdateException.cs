using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.BusinessLogicLayer
{
    public class UpdateException : Exception
    {
        public UpdateException(string message) : base(message)
        {
        }
    }
}
