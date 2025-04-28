using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetUsersAsync(int userId);
        public Task<User> GetUserByEmailAsync(string email, int userId);
        public Task<string> GetUserRole(int id);
        public Task<User> CreateUserAsync(User userToBeCreated);
        public Task<User> UpdateUserAsync(int id, User userToBeUpdated, int userId);
        public Task<bool> DeleteUserAsync(int id, int userId);
        //public Task<bool> AuthenticateAsync(string email, string password);
    }
}
