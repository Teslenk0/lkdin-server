using LKDin.Server.Domain;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User Create(User user)
        {
            LKDinDataManager.AddDataToStore<User>(user);

            return user;
        }

        public bool Exists(string id)
        {
            return LKDinDataManager.Users.Any(u => u.Id == id);
        }

        public User? Get(string id)
        {
            User user;

            /*using (var context = DbContextFactory.Create())
            {
                user = context.Users.Find(id);
            } */

            return null;
        }
    }
}
