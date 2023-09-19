using CourseDesk.Models;
using CourseDesk.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace CourseDesk.Repositories
{
    /// <summary>
    /// Represents the session 
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly DbConnection _context;
        public UserRepository(DbConnection context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns the user based on the the username
        /// </summary>
        /// <param name="SessionUser"></param>
        /// <returns></returns>
        public User GetUserByUsername(User SessionUser)
        {
            User user = _context.Users.Where(u => u.Username == SessionUser.Username).FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get user based on username and password
        /// </summary>
        /// <param name="sessionUser"></param>
        /// <returns></returns>
        public User GetUserByUsernameAndPassword(User sessionUser)
        {
            User userObj = _context.Users.Where(u => u.Username == sessionUser.Username && u.Password == sessionUser.Password).FirstOrDefault();
            
            return userObj;
        }

        /// <summary>
        /// Adds the user to the database
        /// </summary>
        /// <param name="User"></param>
        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public string GetUserType(User user)
        {
            return user.User_type;
        }
    }
}
