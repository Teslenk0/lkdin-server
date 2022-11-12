using LKDin.Logging.Service.Internal.Logging;

namespace LKDin.Logging.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Iniciando servicio consumidor de logs...");
            Console.WriteLine("---------------------------------------");

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            var loggingListener = new LoggingListener();

            var connected = loggingListener.StartListening();

            if (connected)
            {
                app.Lifetime.ApplicationStopping.Register(() => loggingListener.StopListening());

                app.Run();
            }

        }

    }
}