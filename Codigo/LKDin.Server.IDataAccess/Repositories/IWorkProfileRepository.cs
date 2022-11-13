using LKDin.Server.Domain;

namespace LKDin.Server.IDataAccess.Repositories
{
    public interface IWorkProfileRepository
    {
        public WorkProfile Create(WorkProfile workProfile);

        public bool ExistsByUserId(string userId);

        public WorkProfile AssignImageToWorkProfile(WorkProfile workProfile);

        public WorkProfile UnAssignImageFromWorkProfile(WorkProfile workProfile);

        public WorkProfile GetByUserId(string userId);

        public List<WorkProfile> GetByIds(List<string> workProfileIds);

        public List<WorkProfile> GetByDescription(string description);

        public WorkProfile UpdateWorkProfile(WorkProfile workProfile);

        public void DeleteWorkProfileByUserId(string userId);
    }
}