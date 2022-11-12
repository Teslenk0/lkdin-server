using Grpc.Core;
using LKDin.Logging.Client;

namespace LKDin.Server.V2.Services
{
    public class UserService : User.UserBase
    {
        private readonly Logger _logger;
    
        public UserService()
        {
            _logger = new Logger("server-v2:grpc-services:user");
        }

        public override Task<ActionReply> CreateUser(UpsertUserRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ActionReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<ActionReply> UpdateUser(UpsertUserRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ActionReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<ActionReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ActionReply
            {
                Message = "Hello " + request.Id
            });
        }
    }
}