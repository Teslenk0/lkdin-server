using LKDin.Server.Domain;

namespace LKDin.DTOs
{
    public class SkillDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public static SkillDTO EntityToDTO(Skill skill)
        {
            return new SkillDTO()
            {
                Id = skill.Id.ToString(),
                Name = skill.Name
            };
        }

        public static Skill DTOToEntity(SkillDTO skillDTO)
        {
            var skill = new Skill()
            {
                Name = skillDTO.Name
            };

            if (skillDTO.Id != null)
            {
                skill.Id = Guid.Parse(skillDTO.Id);
            }

            return skill;
        }
    }
}