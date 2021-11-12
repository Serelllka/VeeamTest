using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Server.Entity;

namespace Server.Services
{
    public class SessionManager
    {
        private Logger _logger;
        private string _ipAddress;
        private int _firstSessionPort;
        private int _secondSessionPort;
        private List<User> _users;

        public SessionManager(string ip, int firstPort, int secondPort, Logger logger)
        {
            _logger = logger;
            _ipAddress = ip;
            _firstSessionPort = firstPort;
            _secondSessionPort = secondPort;
            _users = new List<User>();
        }
        
        public void StartFirstSession()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse(_ipAddress), _firstSessionPort);
            listener.Start();

            while(true)
            {
                var client = listener.AcceptTcpClient();
                var session = new FirstSession(client, _users);
                
                var clientThread = new Thread(session.Start);
                clientThread.Start();
            }
        }

        public void StartSecondSession()
        {
            var listener = new TcpListener(IPAddress.Parse(_ipAddress), _secondSessionPort);
            listener.Start();

            while(true)
            {
                TcpClient client = listener.AcceptTcpClient();
                SecondSession session = new SecondSession(client, _users, _logger);
                
                Thread clientThread = new Thread(new ThreadStart(session.Start));
                clientThread.Start();
            }
        }
    }
}