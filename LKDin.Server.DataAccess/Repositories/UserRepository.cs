using LKDin.Server.Domain;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User Create(User user)
        {
            DataManager.AddDataToStore<User>(user);

            return user;
        }

        public bool Exists(string id)
        {
            return DataManager.Users.Any(u => u.Id.Equals(id));
        }

        public User? Get(string id)
        {
            return DataManager.Users.Find(u => u.Id.Equals(id));
        }
    }
}
