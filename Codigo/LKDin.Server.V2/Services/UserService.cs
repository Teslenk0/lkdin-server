using AutoMapper;
using Grpc.Core;
using LKDin.DTOs;
using LKDin.IBusinessLogic;
using LKDin.Logging.Client;

namespace LKDin.Server.V2.Services
{
    public class UserService : User.UserBase
    {
        private readonly Logger _logger;

        private readonly IUserLogic _userLogic;

        private readonly IMapper _mapper;

        public UserService(IUserLogic userLogic, IMapper mapper)
        {
            _logger = new Logger("server-v2:grpc-services:user");

            _userLogic = userLogic;

            _mapper = mapper;
        }

        public override async Task<ActionReply> CreateUser(UpsertUserRequest request, ServerCallContext context)
        {
            _logger.Info("Recibida nueva llamada gRPC para crear usuario");

            var userDTO = _mapper.Map<UserDTO>(request);

            await _userLogic.CreateUser(userDTO);

            return ConfirmOperation();
        }

        public override async Task<ActionReply> UpdateUser(UpsertUserRequest request, ServerCallContext context)
        {
            _logger.Info("Recibida nueva llamada gRPC para actualizar usuario");

            var userDTO = _mapper.Map<UserDTO>(request);

            await _userLogic.UpdateUser(userDTO);

            return ConfirmOperation();
        }

        public override async Task<ActionReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            _logger.Info("Recibida nueva llamada gRPC para eliminar usuario");

            var userDTO = _mapper.Map<UserDTO>(request);

            await _userLogic.DeleteUser(userDTO);

            return ConfirmOperation();
        }

        private ActionReply ConfirmOperation()
        {
            return new ActionReply() { Message = "Operación realizada exitosamente", Status = StatusCode.OK.ToString(), Success = true };
        }
    }
}