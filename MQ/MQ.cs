
using RabbitMQ.Client;

namespace MQ
{
    public class MQ : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MQ(string hostName = "localhost", string userName = "guest", string password = "guest")
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password
            };


            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public IModel Channel => _channel;

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}