using System;
using LKDin.Server.Domain;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.DataAccess.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        public List<Skill> CreateMany(List<Skill> skills)
        {
            foreach(Skill skill in skills)
            {
                skill.Id = Guid.NewGuid().ToString();

                LKDinDataManager.AddDataToStore<Skill>(skill);
            }

            return skills;
        }
    }
}

