using System.IO;
using System.Threading;
using Server.Logger;
using Server.Services;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            const int firstPort = 8000;
            const int secondPort = 8001;

            var logger = new FileLogger("logger.log");
            var sessionManager = new SessionManager("127.0.0.1", firstPort, secondPort, logger);
            
            var firstSession = new Thread(sessionManager.StartFirstSession);
            firstSession.Start();

            var secondSession = new Thread(sessionManager.StartSecondSession);
            secondSession.Start();
        }
    }
}