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
    }
}
