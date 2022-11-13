using AutoMapper;
using Grpc.Net.Client;
using LKDin.DTOs;
using LKDin.Helpers.Configuration;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LKDin.Admin.Controllers.Api.V1
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly User.UserClient _client;

        private readonly IMapper _mapper;

        public UserController(IMapper mapper)
        {
            var channel = GrpcChannel.ForAddress(ConfigManager.GetConfig<string>(ConfigConstants.GRPC_SERVER_URL));

            _client = new User.UserClient(channel);

            _mapper = mapper;
        }

        // POST api/v1/user
        [HttpPost]
        public async Task<ActionReply> Post([FromBody] UserDTO userDTO)
        {
            var upsertReq = _mapper.Map<UpsertUserRequest>(userDTO);

            return await _client.CreateUserAsync(upsertReq);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionReply> Put(string id, [FromBody] UserDTO userDTO)
        {
            userDTO.Id = id;

            var upsertReq = _mapper.Map<UpsertUserRequest>(userDTO);

            return await _client.UpdateUserAsync(upsertReq);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionReply> Delete(string id)
        {
            var req = new DeleteUserRequest() { Id = id };

            return await _client.DeleteUserAsync(req);
        }
    }
}
