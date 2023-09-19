using CourseDesk.Models;

namespace CourseDesk.Repositories
{
    public interface IUserRepository
    {
        public User GetUserByUsername(User user);
        public User GetUserByUsernameAndPassword(User user);
        public void AddUser(User user);
        public string GetUserType(User user);

    }
}
