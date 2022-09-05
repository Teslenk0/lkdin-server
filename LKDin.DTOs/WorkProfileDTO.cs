using LKDin.Server.Domain;

namespace LKDin.DTOs
{
    public class WorkProfileDTO
    {
        public string Id { get; set; }

        public List<SkillDTO> Skills { get; set; }

        public string Description { get; set; }

        public string? ImagePath { get; set; }

        public UserDTO User { get; set; }

        public string UserId { get; set; }

        public static WorkProfileDTO EntityToDTO(WorkProfile workProfile)
        {
            var workProfileDTO = new WorkProfileDTO()
            {
                Description = workProfile.Description,
                ImagePath = workProfile.ImagePath,
                UserId = workProfile.UserId,
                Id = workProfile.Id.ToString(),
                Skills = new List<SkillDTO>()
            };

            if (workProfile.User != null)
            {
                workProfileDTO.User = UserDTO.EntityToDTO(workProfile.User);
            }

            if (workProfile.Skills != null)
            {
                workProfile.Skills.ToList().ForEach(skill =>
                {
                    workProfileDTO.Skills.Add(SkillDTO.EntityToDTO(skill));
                });
            }

            return workProfileDTO;
        }

        public static WorkProfile DTOToEntity(WorkProfileDTO workProfileDTO)
        {
            var workProfile = new WorkProfile()
            {
                Description = workProfileDTO.Description,
                ImagePath = workProfileDTO.ImagePath,
                UserId = workProfileDTO.UserId,
                Skills = new List<Skill>()
            };

            if(workProfileDTO.Id != null)
            {
                workProfile.Id = Guid.Parse(workProfileDTO.Id);
            } 

            if (workProfileDTO.User != null)
            {
                workProfile.User = UserDTO.DTOToEntity(workProfileDTO.User);
            }

            if (workProfileDTO.Skills != null)
            {
                workProfileDTO.Skills.ToList().ForEach(skill =>
                {
                    workProfile.Skills.Add(SkillDTO.DTOToEntity(skill));
                });
            }

            return workProfile;
        }
    }
}