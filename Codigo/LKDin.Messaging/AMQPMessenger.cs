using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Threading.Channels;

namespace LKDin.Messaging
{
    public class AMQPMessenger
    {
        private readonly string _hostname;

        private readonly int _port;

        private readonly string _password;

        private readonly string _username;

        private readonly string _exchangeName;

        private IConnection _connection;

        private IModel _channel;

        private Dictionary<string, Subject<Message>> _queuesSubjects;

        public AMQPMessenger(string hostname, int port, string username, string password, string exchangeName)
        {
            _hostname = hostname;
            _port = port;
            _username = username;
            _password = password;
            _exchangeName = exchangeName;

            _queuesSubjects = new Dictionary<string, Subject<Message>>();

            CreateConnection();
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password,
                    Port     = _port,
                    AutomaticRecoveryEnabled = true,
                    TopologyRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(3)
                };

                _connection = factory.CreateConnection();

                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falló la conexión AMQP al servidor de mensajeria - Error: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            return _connection != null && _channel != null && _channel.IsOpen && _connection.IsOpen;
        }

        private bool EnsureQueueExists(string queueName)
        {
            _channel.ExchangeDeclare(_exchangeName, "topic", true, false, null);

            _channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: queueName);

            if (!_queuesSubjects.ContainsKey(queueName))
            {
                _queuesSubjects[queueName] = new Subject<Message>();
            }

            return true;
        }

        public void SendMessage(Message message)
        {
            try
            {
                if (ConnectionExists() && EnsureQueueExists(message.Route))
                {
                    var jsonMessage = JsonSerializer.Serialize(message);

                    byte[] messageBytes = Encoding.UTF8.GetBytes(jsonMessage);

                    _channel.BasicPublish(exchange: _exchangeName,
                                     routingKey: message.Route,
                                     basicProperties: null,
                                     body: messageBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falló el envio del mensaje: {message} - Error: {ex.Message}");
            }
        }

        public Subject<Message> GetObservable(string queue)
        {
            return _queuesSubjects[queue];
        }

        public bool StartReceivingMessages(string queue)
        {
            try
            {
                if (ConnectionExists() && EnsureQueueExists(queue))
                {
                    var consumer = new EventingBasicConsumer(_channel);

                    consumer.Received += (sender, eventArgs) =>
                    {
                        var body = eventArgs.Body.ToArray();

                        var stringifiedMessage = Encoding.UTF8.GetString(body);

                        var message = JsonSerializer.Deserialize<Message>(stringifiedMessage);

                        _queuesSubjects[queue].OnNext(message);
                    };

                    _channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);

                    return true;
                } else
                {
                    Console.WriteLine($"Error: No hay una conexión abierta al servidor de mensajeria");

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falló la recepcion del mensaje - Error: {ex.Message}");

                return false;
            }
        }
    }
}