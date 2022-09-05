using LKDin.Server.Domain;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.DataAccess.Repositories
{
    public class WorkProfileRepository : IWorkProfileRepository
    {

        public WorkProfile Create(WorkProfile workProfile)
        {
            using (var context = DbContextFactory.Create())
            {
                context.WorkProfiles.Add(workProfile);

                context.SaveChanges();
            }

            return workProfile;
        }

        public bool ExistsByUserId(string userId)
        {
            bool exists;

            using (var context = DbContextFactory.Create())
            {
                exists = context.WorkProfiles.Any(u => u.UserId == userId);
            }

            return exists;
        }

        public WorkProfile Update(WorkProfile workProfile)
        {
            using (var context = DbContextFactory.Create())
            {
                context.WorkProfiles.Update(workProfile);

                context.SaveChanges();
            }

            return workProfile;
        }

        public WorkProfile? GetByUserId(string userId)
        {
            WorkProfile workProfile;

            using (var context = DbContextFactory.Create())
            {
                workProfile = context
                                .WorkProfiles
                                .Where(wp => wp.UserId == userId)
                                .FirstOrDefault();
            }

            return workProfile;
        }
    }
}
