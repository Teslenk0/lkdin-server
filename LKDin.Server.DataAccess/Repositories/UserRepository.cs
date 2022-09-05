using LKDin.Server.Domain;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User Create(User user)
        {
            using (var context = DbContextFactory.Create())
            {
                context.Users.Add(user);

                context.SaveChanges();
            }

            return user;
        }

        public bool Exists(string id)
        {
            bool exists;

            using (var context = DbContextFactory.Create())
            {
                exists = context.Users.Any(u => u.Id == id);
            }

            return exists;
        }

        public User? Get(string id)
        {
            User user;

            using (var context = DbContextFactory.Create())
            {
                user = context.Users.Find(id);
            }

            return user;
        }
    }
}
