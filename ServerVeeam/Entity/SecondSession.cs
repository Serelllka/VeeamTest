
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Server.Logger;
using Veeam.Configuration;

namespace Server.Entity
{
    public class SecondSession : Session
    {
        private List<User> _users;
        private TcpClient _client;
        private int _bufferSize;
        private ILogger _logger;

        public SecondSession(TcpClient client,
            List<User> users,
             ILogger logger,
            int bufferSize = 1024) 
        : base(client)
        {
            _logger = logger;
            _users = users;
            _client = client;
            _bufferSize = bufferSize;
        }

        public override void Start()
        {
            NetworkStream stream = _client.GetStream();

            string userId = ReceiveMessage(stream);
            if (_users.All(item => item.Id != userId))
            {
                SendMessage("This user isn't registered!\n", stream);
                return;
            }
            
            string userCode = ReceiveMessage(stream);
            if (_users.First(item => item.Id == userId).Code != userCode)
            {
                SendMessage("Wrong user code!\n", stream);
                return;
            }
            
            string userMessage = ReceiveMessage(stream);
            SendMessage("Your message received!\n", stream);
            _logger.Log($"{userId},{userCode}: {userMessage}");
        }

        private string ReceiveMessage(NetworkStream stream)
        {
            using var ms = new MemoryStream();
            byte[] data = new byte[_bufferSize];
            int totalReadBytes = 0;
            do
            {
                int read = stream.Read(data, totalReadBytes, _bufferSize);
                ms.Write(data, totalReadBytes, read);
                totalReadBytes += read;
            } while (stream.DataAvailable);

            return Configuration.Encoder.GetString(ms.ToArray());
        }

        private void SendMessage(string message, NetworkStream stream)
        {
            byte[] messageInBytes = Configuration.Encoder.GetBytes(message);
            stream.Write(messageInBytes, 0, messageInBytes.Length);
        }
    }
}