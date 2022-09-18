using LKDin.Helpers.Assets;
using LKDin.Server.Domain;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.DataAccess.Repositories
{
    public class WorkProfileRepository : IWorkProfileRepository
    {
        public WorkProfile Create(WorkProfile workProfile)
        {
            workProfile.Id = Guid.NewGuid().ToString();

            DataManager.AddDataToStore<WorkProfile>(workProfile);

            return workProfile;
        }

        public bool ExistsByUserId(string userId)
        {
            return DataManager.WorkProfiles.Any(u => u.UserId == userId);
        }

        public WorkProfile AssignImageToWorkProfile(WorkProfile workProfile)
        {
            var assetPath = AssetManager
                    .CopyAssetToAssetsFolder<WorkProfile>(workProfile.ImagePath, workProfile.Id);

            workProfile.ImagePath = assetPath;

            DataManager.UpdateDataFromStore<WorkProfile>(workProfile);

            return workProfile;
        }

        public WorkProfile? GetByUserId(string userId)
        {
            return DataManager.WorkProfiles.Find(wp => wp.UserId == userId);
        }

        public List<WorkProfile> GetByIds(List<string> workProfileIds)
        {
            return DataManager.WorkProfiles
                .Where(wp => workProfileIds
                .Contains(wp.Id))
                .ToList();
        }

        public List<WorkProfile> GetByDescription(string description)
        {
            return DataManager
                .WorkProfiles
                .Where(wp => wp.Description.ToLower().Contains(description.ToLower()))
                .ToList();
        }
    }
}
