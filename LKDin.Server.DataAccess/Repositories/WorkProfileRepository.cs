using LKDin.Server.Domain;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.DataAccess.Repositories
{
    public class WorkProfileRepository : IWorkProfileRepository
    {

        public WorkProfile Create(WorkProfile workProfile)
        {
            workProfile.Id = Guid.NewGuid().ToString();

            LKDinDataManager.AddDataToStore<WorkProfile>(workProfile);

            return workProfile;
        }

        public bool ExistsByUserId(string userId)
        {
            return LKDinDataManager.WorkProfiles.Any(u => u.UserId == userId);
        }

        public WorkProfile Update(WorkProfile workProfile)
        {
            LKDinDataManager.UpdateDataFromStore<WorkProfile>(workProfile);

            return workProfile;
        }

        public WorkProfile? GetByUserId(string userId)
        {
            return LKDinDataManager.WorkProfiles.Find(wp => wp.UserId == userId);
        }
    }
}
