using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json;

namespace LKDin.Admin.Internal.Filters
{
    public class ErrorHandlerFilter : Attribute, IExceptionFilter
    {
        public ErrorHandlerFilter() : base()
        {
        }

        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;

            string exceptionMessage = exception.Message;

            var exceptionType = exception.GetType();

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            if (exceptionType == typeof(RpcException))
            {
                var rpcException = (RpcException)exception;

                exceptionMessage = rpcException.Status.Detail;

                switch (rpcException.StatusCode)
                {
                    case StatusCode.AlreadyExists:
                        statusCode = HttpStatusCode.Conflict;
                        break;
                    case StatusCode.NotFound:
                        statusCode = HttpStatusCode.NotFound;
                        break;
                    case StatusCode.PermissionDenied:
                        statusCode = HttpStatusCode.Unauthorized;
                        break;
                    case StatusCode.InvalidArgument:
                        statusCode = HttpStatusCode.BadRequest;
                        break;
                }
            }

            var result = JsonSerializer.Serialize(new { Message = exceptionMessage, Status = statusCode, Success = false });

            context.Result = new ContentResult()
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = ((int)statusCode)
            };
        }
    }
}