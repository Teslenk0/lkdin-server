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
        public async Task Post([FromBody] UserDTO userDTO)
        {
            try
            {
                var upsertReq = _mapper.Map<UpsertUserRequest>(userDTO);

                _client.CreateUser(upsertReq);
            }catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }



        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
