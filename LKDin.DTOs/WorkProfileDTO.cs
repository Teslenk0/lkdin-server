using LKDin.Server.Domain;

namespace LKDin.DTOs
{
    public class WorkProfileDTO : ProtectedDTO
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string? ImagePath { get; set; }

        public List<SkillDTO> Skills { get; set; }

        public static WorkProfileDTO EntityToDTO(WorkProfile workProfile)
        {
            var workProfileDTO = new WorkProfileDTO()
            {
                Description = workProfile.Description,
                ImagePath = workProfile.ImagePath,
                UserId = workProfile.UserId,
                Id = workProfile.Id.ToString()
            };

            return workProfileDTO;
        }

        public static WorkProfile DTOToEntity(WorkProfileDTO workProfileDTO)
        {
            var workProfile = new WorkProfile()
            {
                Description = workProfileDTO.Description,
                ImagePath = workProfileDTO.ImagePath,
                UserId = workProfileDTO.UserId,
            };

            if(workProfileDTO.Id != null)
            {
                workProfile.Id = workProfileDTO.Id;
            }

            return workProfile;
        }
    }
}