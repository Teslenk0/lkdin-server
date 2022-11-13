using LKDin.Server.Domain;

namespace LKDin.Server.IDataAccess.Repositories
{
    public interface IUserRepository
    {
        public User Create(User user);

        public bool Exists(string id);

        public User? Get(string id);

        public User Update(User user);

        public void Delete(User user);
    }
}