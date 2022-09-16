using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using webapi_bilheteria_c.Domain.Interface;

namespace webapi_bilheteria_c.Infra.Client
{
    public class RabbitMQClient : IMessageProducer
    {
        public void SendMessage<T> (T message, string queue ){
            var factory = new ConnectionFactory { 
            HostName = "localhost"            
            };       
                 
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            
            channel.BasicPublish(exchange: "", routingKey: queue, body: body);
        }

    }
}