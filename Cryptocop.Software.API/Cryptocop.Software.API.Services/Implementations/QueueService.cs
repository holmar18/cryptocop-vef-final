using System.Runtime.CompilerServices;
using Cryptocop.Software.API.Services.Interfaces;
//using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json.Serialization;
using Cryptocop.Software.API.Models;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        private static ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost", Port=5672 };
        private static IConnection connection = factory.CreateConnection();
        private static IModel channel = connection.CreateModel();

    
        public void PublishMessage(string routingKey, object body, string exch)
        {
            channel.ExchangeDeclare(exchange: exch, type: ExchangeType.Direct, durable: true);

            string stringjson = JsonConvert.SerializeObject(body);
            var mbody = Encoding.UTF8.GetBytes(stringjson);
            channel.BasicPublish(exchange: exch,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: mbody);
            Console.WriteLine(" [x] Sent {0}", stringjson);
        }

        public void Dispose()
        {
            channel.Close();
            connection.Close();
        }
    }
}