using System;
using System.Net.Sockets;
using Test.ValueObject;
using Veeam.Tools;

namespace Test.Entity
{
    public class Client : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public Client(string address, int port, int bufferSize = 1024)
        {
            if (bufferSize <= 0)
            {
                throw new VeeamException("buffer size must be positive");
            }
            BufferSize = bufferSize;
            _client = new TcpClient(address, port);
            _stream = _client.GetStream();
        }
        
        public int BufferSize { get; }

        public void SendSomeData(byte[] data)
        {
            _stream.Write(data, 0, data.Length);
        }

        public DataContainer GrabSomeData()
        {
            byte[] data = new byte[BufferSize];
            int dataSize = _stream.Read(data, 0, data.Length);
            return new DataContainer(data, dataSize);
        }

        public void Dispose()
        {
            _client?.Dispose();
            _stream?.Dispose();
        }
    }
}