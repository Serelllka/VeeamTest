
using System.Collections.Generic;
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
            byte[] data = new byte[_bufferSize];
            NetworkStream stream = _client.GetStream();
            string userId, userCode, userMessage;

            int readBytes;
            StringBuilder stringBuilder = new StringBuilder();
            
            do
            {
                readBytes = stream.Read(data, 0, data.Length);
                stringBuilder.Append(Configuration.Encoder.GetString(data, 0, readBytes));
            } while (stream.DataAvailable);
            userId = stringBuilder.ToString();

            stringBuilder.Clear();
            do
            {
                readBytes = stream.Read(data, 0, data.Length);
                stringBuilder.Append(Configuration.Encoder.GetString(data, 0, readBytes));
            } while (stream.DataAvailable);
            userCode = stringBuilder.ToString();

            stringBuilder.Clear();
            do
            {
                readBytes = stream.Read(data, 0, data.Length);
                stringBuilder.Append(Configuration.Encoder.GetString(data, 0, readBytes));
            } while (stream.DataAvailable);
            userMessage = stringBuilder.ToString();

            byte[] msg;
            if (_users.FirstOrDefault(item => item.Id == userId) is null) 
            {
                msg = Configuration.Encoder.GetBytes("This user isn't registered!\n");
                stream.Write(msg, 0 , msg.Length);
            }
            else if (_users.First(item => item.Id == userId).Code != userCode)
            {
                msg = Configuration.Encoder.GetBytes("Wrong user code!\n");
                stream.Write(msg, 0 , msg.Length);
            }
            else
            {
                msg = Configuration.Encoder.GetBytes("Your message received!\n");
                stream.Write(msg, 0 , msg.Length);
                _logger.Log($"{userId},{userCode}: {userMessage}");
            }
        }
    }
}