using AutoMapper;
using LKDin.DTOs;

namespace LKDin.Server.V2.Internal.Mappings
{
    public class ServerMappingProfile : Profile
    {
        public ServerMappingProfile()
        {
            CreateMap<UserDTO, UpsertUserRequest>();
            CreateMap<UpsertUserRequest, UserDTO>();

            CreateMap<UserDTO, DeleteUserRequest>();
            CreateMap<DeleteUserRequest, UserDTO>();
        }
    }
}
