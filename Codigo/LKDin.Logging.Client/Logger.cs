using LKDin.Helpers.Configuration;
using LKDin.Messaging;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace LKDin.Logging.Client;

public class Logger
{
    private readonly AMQPMessenger _messenger;

    private readonly string _queueName;

    private readonly string _namespace;

    public Logger(string loggingNameSpace)
    {
        _messenger = new AMQPMessenger(
                ConfigManager.GetConfig<string>("BROKER_HOSTNAME"),
                ConfigManager.GetConfig<int>("BROKER_PORT"),
                ConfigManager.GetConfig<string>("BROKER_USER"),
                ConfigManager.GetConfig<string>("BROKER_PASS"),
                ConfigManager.GetConfig<string>("BROKER_EXCHANGE")
        );

        _queueName = ConfigManager.GetConfig<string>("BROKER_QUEUE");

        _namespace = loggingNameSpace;
    }

    public void Info(string message)
    {
        var log = new Log()
        {
            Level = LogLevel.INFO,
            Message = message,
            TimeStamp = DateTime.Now.ToUniversalTime(),
            NameSpace = _namespace
        };

        Log(log);
    }


    public void Error(string message)
    {
        var log = new Log()
        {
            Level = LogLevel.ERROR,
            Message = message,
            TimeStamp = DateTime.Now.ToUniversalTime(),
            NameSpace = _namespace
        };

        Log(log);
    }

    public void Warn(string message)
    {
        var log = new Log()
        {
            Level = LogLevel.WARN,
            Message = message,
            TimeStamp = DateTime.Now.ToUniversalTime(),
            NameSpace = _namespace
        };

        Log(log);
    }

    private void Log(Log log)
    {
        Console.WriteLine($"{log.TimeStamp} | {log.NameSpace} - {log.Message}");

        var message = new Message()
        {
            Body = JsonSerializer.Serialize<Log>(log),
            Id = Guid.NewGuid().ToString(),
            TimeStamp = log.TimeStamp,
            Route = _queueName
        };

        _messenger.SendMessage(message);
    }
}
