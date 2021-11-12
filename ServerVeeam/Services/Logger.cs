using System.IO;
using System.Text;

namespace Server
{
    public class Logger
    {
        private Stream _output;
        
        public Logger(Stream output)
        {
            _output = output;
        }

        public void Log(string message)
        {
            _output.Write(Encoding.UTF8.GetBytes(message));
        }
    }
}