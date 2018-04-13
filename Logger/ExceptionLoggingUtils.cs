using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.Logger
{
    public static class ExceptionLoggingUtils
    {
        private const int MaxDepth = 10;

        public static string ExceptionAsString(Exception ex, int depth = 0)
        {
            if (depth > MaxDepth)
            {
                return $"Max inner exception depth ({MaxDepth}) exceeded, stopping further recursive calls";
            }

            return $"Message: {ex.Message} StackTrace: {ex.StackTrace} {(ex.InnerException != null ? $"InnerException: {ExceptionAsString(ex.InnerException, ++depth)}" : string.Empty)}";
        }
    }
}
