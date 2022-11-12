using LKDin.Helpers.Configuration;
using LKDin.Messaging;
using System.Reactive.Linq;
using System.Text.Json;

namespace LKDin.Logging.Service.Internal.Logging
{
    public class LoggingListener
    {
        private readonly AMQPMessenger _messenger;

        private readonly string _queueName;

        private readonly LoggingService _loggingService;

        private IDisposable _loggingObs;

        public LoggingListener()
        {
            _messenger = new AMQPMessenger(
                ConfigManager.GetConfig<string>(ConfigConstants.BROKER_HOSTNAME),
                ConfigManager.GetConfig<int>(ConfigConstants.BROKER_PORT),
                ConfigManager.GetConfig<string>(ConfigConstants.BROKER_USER),
                ConfigManager.GetConfig<string>(ConfigConstants.BROKER_PASS),
                ConfigManager.GetConfig<string>(ConfigConstants.BROKER_EXCHANGE)
            );

            _queueName = ConfigManager.GetConfig<string>(ConfigConstants.BROKER_QUEUE);

            _loggingService = LoggingService.Instance;
        }

        public bool StartListening()
        {
            var listening = _messenger.StartReceivingMessages(_queueName);

            if (listening)
            {
                _loggingObs = _messenger.GetObservable(_queueName).Subscribe(ProcessMessage);
            }

            return listening;
        }

        private void ProcessMessage(Message message)
        {
            Console.WriteLine("Recibido nuevo mensaje: {0}", message.Id);

            var log = JsonSerializer.Deserialize<Log>(message.Body);

            _loggingService.SaveLog(log);
        }

        public void StopListening()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Finalizando servicio consumidor de logs");
            Console.WriteLine("---------------------------------------");

            _loggingObs.Dispose();
        }
    }
}
