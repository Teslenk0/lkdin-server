using LKDin.Server.Domain;

namespace LKDin.Server.IDataAccess.Repositories
{
    public interface ISkillRepository
    {
        public List<Skill> CreateMany(List<Skill> skills);

        public List<Skill> GetByName(List<string> skillsToSearchFor);

        public List<Skill> GetByWorkProfileIds(List<string> workProfileIds);

        public void DeleteSkillsByWorkProfileId(string workProfileId);
    }
}