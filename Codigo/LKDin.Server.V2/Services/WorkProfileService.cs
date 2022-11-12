using Grpc.Core;
using LKDin.Logging.Client;

namespace LKDin.Server.V2.Services
{
    public class WorkProfileService : WorkProfile.WorkProfileBase
    {
        private readonly Logger _logger;

        public WorkProfileService()
        {
            _logger = new Logger("server-v2:grpc-services:work-profile");
        }

        public override Task<CreateWorkProfileReply> CreateWorkProfile(CreateWorkProfileRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CreateWorkProfileReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<UpdateWorkProfileReply> UpdateWorkProfile(UpdateWorkProfileRequest request, ServerCallContext context)
        {
            return Task.FromResult(new UpdateWorkProfileReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<DeleteWorkProfileReply> DeleteWorkProfile(DeleteWorkProfileRequest request, ServerCallContext context)
        {
            return Task.FromResult(new DeleteWorkProfileReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<DeleteWorkProfileImageReply> DeleteWorkProfileImage(DeleteWorkProfileImageRequest request, ServerCallContext context)
        {
            return Task.FromResult(new DeleteWorkProfileImageReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}