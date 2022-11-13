using AutoMapper;
using Google.Protobuf.Collections;
using LKDin.DTOs;
using LKDin.Helpers.Utils;

namespace LKDin.Admin.Internal.Mappings
{
    public class AdminMappingProfile : Profile
    {
        public AdminMappingProfile()
        {
            CreateMap(typeof(IEnumerable<>), typeof(RepeatedField<>)).ConvertUsing(typeof(EnumerableToRepeatedFieldTypeConverter<,>));

            CreateMap(typeof(RepeatedField<>), typeof(List<>)).ConvertUsing(typeof(RepeatedFieldToListTypeConverter<,>));

            CreateMap<SkillDTO, UpsertSkillRequest>().ReverseMap();
            
            CreateMap<UserDTO, UpsertUserRequest>().ReverseMap();

            CreateMap<UserDTO, DeleteActionRequest>().ReverseMap();

            CreateMap<WorkProfileDTO, DeleteActionRequest>().ReverseMap();

            CreateMap<WorkProfileDTO, UpsertWorkProfileRequest>().ReverseMap();
        }
    }
}
