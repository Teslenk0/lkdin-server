using AutoMapper;
using LKDin.DTOs;

namespace LKDin.Admin.Internal.Mappings
{
    public class AdminMappingProfile : Profile
    {
        public AdminMappingProfile()
        {
            CreateMap<UserDTO, UpsertUserRequest>();
            CreateMap<UpsertUserRequest, UserDTO>();
        }
    }
}
