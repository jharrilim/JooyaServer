using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace JooyaVision.Server
{
    class Program
    {
        private static WebSocketServer<JooyaClient> _jooyaServer;

        public static void Main()
        {
            var listenAddress = new IPEndPoint(IPAddress.Any, 12345);
            using (_jooyaServer = new WebSocketServer<JooyaClient>(listenAddress))
            {
                Console.WriteLine("Started JooyaVision server, press Q to exit.");

                while (Console.ReadKey().Key == ConsoleKey.Q)
                {

                }
            }
        }

        private static void Broadcast(string message)
        {
            Task.Run(async () =>
            {
                try
                {
                    await Task.WhenAll(_jooyaServer.Clients.Select(c => c.SendAsync(message)));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Broadcast error: {e}");
                }
            });
        }

        private class JooyaClient : WebSocketClient
        {
            static List<string> messages = new List<string>();
            protected override void OnMessage(string message)
            {
                Broadcast(message);
                MessageListAppend(message);
            }

            protected override void OnOpen()
            {
                foreach(var m in messages) Send(m);
            }


            private static void MessageListAppend(string message)
            {
                messages.Add(message);
                if (messages.Count > 10) messages.RemoveAt(0);
            }
        }
    }
}
