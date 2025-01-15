using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace GerenciadorDeProjetos.Application.Services
{
    public class RabbitMqConsumer
    {
        private readonly string _hostname = "localhost";
        private readonly string _queueName = "user_queue";

        public void StartConsuming()
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received {message}");
                };

                channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

                Console.WriteLine(" [*] Waiting for messages...");
            }
        }
    }
}