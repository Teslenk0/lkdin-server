using LKDin.Helpers;
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

        public WorkProfile AssignImageToWorkProfile(WorkProfile workProfile)
        {
            var assetPath = LKDinAssetManager
                    .CopyAssetToAssetsFolder<WorkProfile>(workProfile.ImagePath, workProfile.Id);

            workProfile.ImagePath = assetPath;

            LKDinDataManager.UpdateDataFromStore<WorkProfile>(workProfile);

            return workProfile;
        }

        public WorkProfile? GetByUserId(string userId)
        {
            return LKDinDataManager.WorkProfiles.Find(wp => wp.UserId == userId);
        }

        public List<WorkProfile> GetByIds(List<string> workProfileIds)
        {
            return LKDinDataManager.WorkProfiles
                .Where(wp => workProfileIds
                .Contains(wp.Id))
                .ToList();
        }

        public List<WorkProfile> GetByDescription(string description)
        {
            return LKDinDataManager
                .WorkProfiles
                .Where(wp => wp.Description.ToLower().Contains(description.ToLower()))
                .ToList();
        }
    }
}
