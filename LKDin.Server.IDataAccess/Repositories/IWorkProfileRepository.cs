using LKDin.Server.Domain;

namespace LKDin.Server.IDataAccess.Repositories
{
    public interface IWorkProfileRepository
    {
        public WorkProfile Create(WorkProfile workProfile);

        public bool ExistsByUserId(string userId);

        public WorkProfile Update(WorkProfile workProfile);

        public WorkProfile GetByUserId(string userId);
    }
}