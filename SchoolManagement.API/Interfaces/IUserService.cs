using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;
using static SchoolManagement.API.Models.User;

namespace SchoolManagement.API.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDto>> GetUsersAsync(int userId);
        public Task<UserDto> GetUserByEmailAsync(string email, int userId);
        public Task<UserRole> GetUserRole(int id);
        public Task<UserDto> CreateUserAsync(UserInputDto userToBeCreated, int userId);
        public Task<UserDto> UpdateUserAsync(int id, UserInputDto userToBeUpdated, int userId);
        public Task<bool> DeleteUserAsync(int id, int userId);
    }
}
