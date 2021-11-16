using System.IO;

namespace Server.Logger
{
    public class FileLogger : ILogger
    {
        private string _fileName;
        
        public FileLogger(string fileName)
        {
            _fileName = fileName;
        }
        
        public void Log(string logMassage)
        {
            File.AppendAllText(_fileName, logMassage);
        }
    }
}