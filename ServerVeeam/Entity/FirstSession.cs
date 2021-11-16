using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Veeam.Configuration;
using Veeam.Tools;

namespace Server.Entity
{
    public class FirstSession : Session
    {
        private List<User> _users;
        private TcpClient _client;
        private int _bufferSize;

        public FirstSession(
            TcpClient client,
            List<User> users,
            int bufferSize = 1024)
        : base(client)
        {
            _users = users;
            _client = client;
            _bufferSize = bufferSize;
        }

        public override void Start()
        {
            byte[] data = new byte[_bufferSize];
            NetworkStream stream = _client.GetStream();
            int readBytes;
            
            StringBuilder stringBuilder = new StringBuilder();
            do
            {
                readBytes = stream.Read(data, 0, data.Length);
                stringBuilder.Append(Configuration.Encoder.GetString(data, 0, readBytes));
            } while (stream.DataAvailable);

            string message = stringBuilder.ToString();
            if (_users.FirstOrDefault(item => item.Id == message) is not null)
            {
                throw new VeeamException("This client is already registered");
            }
            Guid clientId = Guid.NewGuid();
            _users.Add(new User(message, clientId.ToString()));
            data = Configuration.Encoder.GetBytes(clientId.ToString());
            stream.Write(data, 0, data.Length);
        }
    }
}