using LKDin.Server.Domain;

namespace LKDin.Server.IDataAccess.Repositories
{
    public interface ISkillRepository
    {
        public List<Skill> CreateMany(List<Skill> skills);
    }
}