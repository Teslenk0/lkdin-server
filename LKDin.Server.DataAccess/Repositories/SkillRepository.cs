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

        public List<Skill> GetByName(List<string> skillsToSearchFor)
        {
            bool isNotEmptySearch = false;

            var normalizedSearchCriteria = new List<string>();

            skillsToSearchFor.ForEach(skill => {

                if(skill != "")
                {
                    normalizedSearchCriteria.Add(skill.ToUpper().Trim());
                    isNotEmptySearch = true;
                }
            });

            if (isNotEmptySearch)
            {
                return LKDinDataManager.Skills.Where(skill => normalizedSearchCriteria.Contains(skill.Name.ToUpper())).ToList();
            } else
            {
                return LKDinDataManager.Skills.ToList();
            }
        }

        public List<Skill> GetByWorkProfileIds(List<string> workProfileIds)
        {
            return LKDinDataManager.Skills
                .Where(skill => workProfileIds
                    .Contains(skill.WorkProfileId))
                .ToList();
        }
    }
}

