using SchoolManagement.API.Models;
using static SchoolManagement.API.Models.User;

namespace SchoolManagement.API.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetUsersAsync(int userId);
        public Task<User> GetUserByEmailAsync(string email, int userId);
        public Task<UserRole> GetUserRole(int id);
        public Task<User> CreateUserAsync(User userToBeCreated, int userId);
        public Task<User> UpdateUserAsync(int id, User userToBeUpdated, int userId);
        public Task<bool> DeleteUserAsync(int id, int userId);
    }
}
