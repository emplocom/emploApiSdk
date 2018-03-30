using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.Logger
{
    class ConsoleLogger : BaseLogger
    {
        public override void WriteLine(string line)
        {
            Console.WriteLine(PrependTimeToOutputLine(line));
        }
    }
}
