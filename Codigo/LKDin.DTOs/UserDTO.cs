using LKDin.Server.Domain;

namespace LKDin.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public static UserDTO EntityToDTO(User user)
        {
            var userDTO = new UserDTO()
            {
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
            };

            return userDTO;
        }

        public static User DTOToEntity(UserDTO userDTO)
        {
            var user = new User()
            {
                Id = userDTO.Id,
                Name = userDTO.Name,
                Password = userDTO.Password,
            };

            return user;
        }
    }
}