using LKDin.Server.Domain;

namespace LKDin.DTOs
{
    public class SkillDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string WorkProfileId { get; set; }

        public static SkillDTO EntityToDTO(Skill skill)
        {
            return new SkillDTO()
            {
                Id = skill.Id.ToString(),
                Name = skill.Name,
                WorkProfileId = skill.WorkProfileId.ToString()
            };
        }

        public static Skill DTOToEntity(SkillDTO skillDTO)
        {
            var skill = new Skill()
            {
                Name = skillDTO.Name,
                WorkProfileId = skillDTO.WorkProfileId
            };

            if (skillDTO.Id != null)
            {
                skill.Id = skillDTO.Id;
            }

            return skill;
        }
    }
}