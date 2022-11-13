using Grpc.Core.Interceptors;
using Grpc.Core;
using LKDin.Logging.Client;
using LKDin.Exceptions;

namespace LKDin.Server.V2.Internal.Interceptors
{
    public class GRPCErrorInterceptor : Interceptor
    {
        private readonly Logger _logger;

        public GRPCErrorInterceptor()
        {
            _logger = new Logger("server-v2:grpc-interceptors:error");
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al llamar {context.Method} - Error: {ex.Message}");

                var exceptionType = ex.GetType();

                var exceptionMessage = ex.Message;

                StatusCode rpcCode = StatusCode.Internal;

                if (exceptionType == typeof(UserAlreadyExistsException) ||
                    exceptionType == typeof(WorkProfileAlreadyExistsException) ||
                    exceptionType == typeof(EntityAlreadyExistsException))
                {
                    rpcCode = StatusCode.AlreadyExists;
                }
                else if (exceptionType == typeof(AssetDoesNotExistException) || exceptionType == typeof(UserDoesNotExistException) || exceptionType == typeof(WorkProfileDoesNotExistException))
                {
                    rpcCode = StatusCode.NotFound;
                }
                else if (exceptionType == typeof(UnauthorizedException))
                {
                    rpcCode = StatusCode.PermissionDenied;
                } else if (exceptionType == typeof(NoImageAssignedException))
                {
                    rpcCode = StatusCode.InvalidArgument;
                }

                throw new RpcException(new Status(rpcCode, exceptionMessage));
            }
        }

    }
}
