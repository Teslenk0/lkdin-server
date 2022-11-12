using LKDin.Server.Networking;
using LKDin.Server.V2.Services;
using LKDin.Helpers.Configuration;
using LKDin.Logging.Client;

namespace LKDin.Server.V2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = new Logger("server-v2");

            logger.Info("Iniciando servidor V2...");

            var networkingManager = ServerNetworkingManager.Instance;

            var connected = await networkingManager.InitTCPConnection();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpc();

            var app = builder.Build();

            AttachServices(app);

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            if (connected)
            {
                logger.Info($"Información del servidor en: {ConfigManager.GetAppDataBasePath()}");

                // We don't need to await this.
                networkingManager.AcceptTCPConnections(ServerConnectionHandler.HandleConnection);

                await app.RunAsync();
            }
        }

        private static void AttachServices(WebApplication app)
        {
            app.MapGrpcService<UserService>();
            app.MapGrpcService<WorkProfileService>();
        }

        private static void SetupServicesToInject(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IMyDependency, MyDependency>();
        }
    }
}