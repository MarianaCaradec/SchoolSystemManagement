using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task<User> GetUserByEmailAsync(string email);
        public Task<string> GetUserRole(int id);
        public Task<User> CreateUserAsync(User userToBeCreated);
        public Task<User> UpdateUserAsync(int id, User userToBeUpdated);
        public Task<bool> DeleteUserAsync(int id);
        public Task<bool> AuthenticateAsync(string email, string password);
    }
}
