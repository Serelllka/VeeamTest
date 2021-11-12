using System.Net.Sockets;

namespace Server.Entity
{
    public abstract class Session
    {
        protected Session(TcpClient client)
        {
            Client = client;
        }
        
        public TcpClient Client { get; }
        public abstract void Start();
    }
}