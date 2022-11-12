using AutoMapper;
using LKDin.DTOs;

namespace LKDin.Admin.Internal
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
