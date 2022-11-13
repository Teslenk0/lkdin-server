using AutoMapper;
using Grpc.Net.Client;
using LKDin.DTOs;
using LKDin.Helpers.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace LKDin.Admin.Controllers.Api.V1
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class WorkProfileController : ControllerBase
    {

        private readonly WorkProfile.WorkProfileClient _client;

        private readonly IMapper _mapper;

        public WorkProfileController(IMapper mapper)
        {
            var channel = GrpcChannel.ForAddress(ConfigManager.GetConfig<string>(ConfigConstants.GRPC_SERVER_URL));

            _client = new WorkProfile.WorkProfileClient(channel);

            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionReply> Post([FromBody] WorkProfileDTO workProfileDTO)
        {
            var upsertReq = _mapper.Map<UpsertWorkProfileRequest>(workProfileDTO);

            return await _client.CreateWorkProfileAsync(upsertReq);
        }

        [HttpPut("{id}")]
        public async Task<ActionReply> Put(string id, [FromBody] WorkProfileDTO workProfileDTO)
        {
            workProfileDTO.Id = id;

            workProfileDTO.UserId = id;

            var upsertReq = _mapper.Map<UpsertWorkProfileRequest>(workProfileDTO);

            return await _client.UpdateWorkProfileAsync(upsertReq);
        }

        [HttpDelete("{id}")]
        public async Task<ActionReply> Delete(string id)
        {
            var req = new DeleteActionRequest() { Id = id };

            return await _client.DeleteWorkProfileAsync(req);
        }

        [HttpDelete("{id}/image")]
        public async Task<ActionReply> DeleteImage(string id)
        {
            var req = new DeleteActionRequest() { Id = id };

            return await _client.DeleteWorkProfileImageAsync(req);
        }
    }
}
