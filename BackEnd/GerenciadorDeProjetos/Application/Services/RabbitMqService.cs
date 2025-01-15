using RabbitMQ.Client;
using System;
using System.Text;

public class RabbitMqService
{
    private readonly string _hostname = "localhost";
    private readonly string _queueName = "user_queue";

    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory() { HostName = _hostname };

        using var connection = factory.CreateConnection();

        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);

    }
}
