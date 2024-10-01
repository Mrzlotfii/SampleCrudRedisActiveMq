using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace CrudRedisActiveMQ.Messaging
{
    public class ActiveMqService
    {
        private readonly IConnectionFactory _connectionFactory;

        public ActiveMqService(string brokerUri)
        {
            _connectionFactory = new ConnectionFactory(brokerUri);
        }

        public void SendMessage(string queueName, string message)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Start();
            using var session = connection.CreateSession();
            var destination = session.GetQueue(queueName);
            using var producer = session.CreateProducer(destination);
            var textMessage = producer.CreateTextMessage(message);
            producer.Send(textMessage);
        }

        public void ReceiveMessage(string queueName, Action<string> onMessageReceived)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Start();
            using var session = connection.CreateSession();
            var destination = session.GetQueue(queueName);
            using var consumer = session.CreateConsumer(destination);

            consumer.Listener += message =>
            {
                if (message is ITextMessage textMessage)
                {
                    onMessageReceived(textMessage.Text);
                }
            };
        }
    }
}
