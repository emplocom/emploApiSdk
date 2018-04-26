using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.Logger
{
    public class EmploApiClientFatalException : Exception
    {
        public EmploApiClientFatalException()
        {
        }

        public EmploApiClientFatalException(string message)
            : base(message)
        {
        }

        public EmploApiClientFatalException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
