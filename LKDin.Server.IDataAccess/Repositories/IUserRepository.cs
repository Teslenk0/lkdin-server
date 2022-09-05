using LKDin.Server.Domain;

namespace LKDin.Server.IDataAccess.Repositories
{
    public interface IUserRepository
    {
        public User Create(User user);
    }
}