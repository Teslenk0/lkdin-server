using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.IBusinessLogic;
using LKDin.Logging.Client;
using LKDin.Server.DataAccess.Repositories;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.BusinessLogic
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly Logger _logger;

        public UserService()
        {
            this._userRepository = new UserRepository();

            this._logger = new Logger("server:business-logic:user-service");
        }

        public async Task CreateUser(UserDTO userDTO)
        {
            this._logger.Info($"Creando usuario ID:{userDTO.Id}");

            var exists = this._userRepository.Exists(userDTO.Id);

            if (exists)
            {
                this._logger.Error($"Usuario ID:{userDTO.Id} ya existe");

                throw new UserAlreadyExistsException(userDTO.Id);
            }

            var user = UserDTO.DTOToEntity(userDTO);

            user.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

            this._userRepository.Create(user);

            this._logger.Info($"Se creó el usuario ID:{userDTO.Id} exitosamente");
        }

        public async Task<UserDTO?> GetUser(string userId)
        {
            this._logger.Info($"Obteniendo usuario ID:{userId}");

            var user = this._userRepository.Get(userId);

            if (user != null)
            {
                return UserDTO.EntityToDTO(user);
            }

            this._logger.Warn($"Usuario inexistente ID:{userId}");

            return null;
        }

        public async Task<UserDTO> ValidateUserCredentials(string userId, string password)
        {
            var user = this._userRepository.Get(userId);

            if (user == null)
            {
                this._logger.Error($"Usuario inexistente ID:{userId}");

                throw new UserDoesNotExistException(userId);
            }

            bool verified = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!verified)
            {
                this._logger.Error($"Contrasena incorrecta para el usuario ID:{userId}");

                throw new UnauthorizedException();
            }

            return UserDTO.EntityToDTO(user);
        }
    }
}