using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MQ
{
    public class RabbitMqListener
    {
        private readonly IModel _channel;
        private readonly OrderConsumerService _consumerService;

        public RabbitMqListener(OrderConsumerService consumerService)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _consumerService = consumerService;

            _channel.QueueDeclare(queue: "order_queue", durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var orderList = JsonConvert.DeserializeObject<List<OrderMessageDto>>(json);

                await _consumerService.HandleMessageAsync(orderList);
            };

            _channel.BasicConsume(queue: "order_queue", autoAck: true, consumer: consumer);
        }
    }

}
