using System;
using System.Text;
using Test.Entity;
using Test.ValueObject;
using Veeam.Configuration;

namespace Test
{
    static class Program
    {
        const string address = "127.0.0.1";
        
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your port:");
            int port = Int32.Parse(Console.ReadLine() ?? string.Empty);
            Client client = new Client(address, port);
            
            Console.Write("Enter your id: ");
            string userId = Console.ReadLine();
            
            if (userId != null)
            {
                byte[] data = Configuration.Encoder.GetBytes(userId);
                client.SendSomeData(data);
            }
            
            StringBuilder builder = new StringBuilder();
            if (port == 8000)
            {
                DataContainer grabData;
                do
                {
                    grabData = client.GrabSomeData();
                    builder.Append(Configuration.Encoder.GetString(grabData.Data, 0, grabData.DataSize));
                } while (grabData.DataSize == client.BufferSize);

                string userCode = builder.ToString();
                Console.WriteLine("User code: {0}", userCode);
            }

            if (port == 8001)
            {
                Console.Write("Enter your code: ");
                string userCode = Console.ReadLine();
                if (userCode != null)
                {
                    byte[] data = Configuration.Encoder.GetBytes(userCode);
                    client.SendSomeData(data);
                }
                
                Console.Write("Enter your message: ");
                string userMessage = Console.ReadLine();
                if (userMessage != null)
                {
                    byte[] data = Configuration.Encoder.GetBytes(userMessage);
                    client.SendSomeData(data);
                }

                DataContainer grabData;
                do
                {
                    grabData = client.GrabSomeData();
                    builder.Append(Configuration.Encoder.GetString(grabData.Data, 0, grabData.DataSize));
                } while (grabData.DataSize == client.BufferSize);

                var result = builder.ToString();
                Console.WriteLine("Result: {0}", result);
            }
        }
    }
}