using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.IBusinessLogic;
using LKDin.Logging.Client;
using LKDin.Server.DataAccess.Repositories;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.BusinessLogic
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserRepository _userRepository;

        private readonly Logger _logger;

        private readonly bool _skipPasswordCheck;

        public UserLogic(bool skipPasswordCheck = false)
        {
            this._userRepository = new UserRepository();

            this._logger = new Logger("server:business-logic:user-service");

            _skipPasswordCheck = skipPasswordCheck;
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

        public async Task UpdateUser(UserDTO userDTO)
        {
            this._logger.Info($"Actualizando usuario ID:{userDTO.Id}");

            var exists = this._userRepository.Exists(userDTO.Id);

            if (!exists)
            {
                this._logger.Error($"Usuario ID:{userDTO.Id} no existe");

                throw new UserDoesNotExistException(userDTO.Id);
            }

            var user = UserDTO.DTOToEntity(userDTO);

            user.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

            this._userRepository.Update(user);

            this._logger.Info($"Se actualizó el usuario ID:{userDTO.Id} exitosamente");
        }

        public async Task DeleteUser(UserDTO userDTO)
        {
            this._logger.Info($"Eliminando usuario ID:{userDTO.Id}");

            var exists = this._userRepository.Exists(userDTO.Id);

            if (!exists)
            {
                this._logger.Error($"Usuario ID:{userDTO.Id} no existe");

                throw new UserDoesNotExistException(userDTO.Id);
            }

            var user = UserDTO.DTOToEntity(userDTO);

            this._userRepository.Delete(user);

            this._logger.Info($"Se eliminó el usuario ID:{userDTO.Id} exitosamente");
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

            if (_skipPasswordCheck)
            {
                return UserDTO.EntityToDTO(user);
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