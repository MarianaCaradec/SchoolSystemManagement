using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Services
{
    public class UserService(SchoolSysDBContext context) : IUserService
    {
        private readonly SchoolSysDBContext _context = context;

        public async Task<IEnumerable<User>> GetUsersAsync(int userId)
        {
            string userRole = await GetUserRole(userId);

            IEnumerable<User> users;

            if (userRole == "Student" || userRole == "Teacher")
            {
                users = await _context.Users
                    .Include(u => u.Email)
                    .Include(u => u.Role)
                    .Select(u => new User
                    {
                        Email = u.Email,
                        Role = u.Role
                    }).ToListAsync();
            }
            else
            {
                users = await _context.Users
                    .Include(u => u.Email)
                    .Include(u => u.Password)
                    .Include(u => u.Role)
                    .Include(u => u.Teacher)
                    .Include(u => u.Student)
                    .Select(u => new User
                    {
                        Email = u.Email,
                        Password = u.Password,
                        Role = u.Role,
                        Teacher = u.Teacher,
                        Student = u.Student
                    }).ToListAsync();
            }

            return users;
        }

        public async Task<User> GetUserByEmailAsync(string email, int userId)
        {
            string userRole = await GetUserRole(userId);

            User userByEmail;

            if (userRole == "Student" || userRole == "Teacher")
            {
                userByEmail = await _context.Users
                    .Include(u => u.Email)
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            else
            {
                userByEmail = await _context.Users
                    .Include(u => u.Email)
                    .Include(u => u.Password)
                    .Include(u => u.Role)
                    .Include(u => u.Teacher)
                    .Include(u => u.Student)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }

            return userByEmail;
        }

        public async Task<string> GetUserRole(int id)
        {
            User user = await _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) throw new KeyNotFoundException($"User with ID {id} not found");

            return user.Role;
        }

        public async Task<User> CreateUserAsync(User userToBeCreated)
        {
            User createdUser = new User
            {
                Email = userToBeCreated.Email,
                Password = userToBeCreated.Password,
                Role = char.ToUpper(userToBeCreated.Role[0]) + userToBeCreated.Role.Substring(1).ToLower()
            };

            if (createdUser.Id == null || createdUser.Id == 0)
            {
                Random random = new Random();
                createdUser.Id = random.Next(1, int.MaxValue);
            }

            _context.Users.Add(userToBeCreated);
            await _context.SaveChangesAsync();

            return createdUser;
        }

        public async Task<User> UpdateUserAsync(int id, User userToBeUpdated, int userId)
        {
            string userRole = await GetUserRole(userId);

            User updatedUser = await _context.Users
                .Include(u => u.Email)
                .Include(u => u.Password)
                .Include(u => u.Role)
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (updatedUser == null) throw new KeyNotFoundException($"User with ID {id} not found");

            if (userRole == "Teacher")
            {
                updatedUser.Email = userToBeUpdated.Email;
                updatedUser.Password = userToBeUpdated.Password;
            }
            else
            {
                updatedUser.Email = userToBeUpdated.Email;
                updatedUser.Password = userToBeUpdated.Password;
                updatedUser.Role = userToBeUpdated.Role;
            }

            _context.Update(updatedUser);   
            await _context.SaveChangesAsync();

            return updatedUser;
        }

        public async Task<bool> DeleteUserAsync(int id, int userId)
        {
            string userRole = await GetUserRole(userId);

            if (userRole == "Student" || userRole == "Teacher")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            User userToBeDeleted = await _context.Users.FindAsync(id);

            if (userToBeDeleted == null) throw new KeyNotFoundException($"User with ID {id} not found");
            
            _context.Users.Remove(userToBeDeleted);
            await _context.SaveChangesAsync();
            
            return true;
        }

        //public async Task<bool> AuthenticateAsync(string email, string password)
        //{
        //    var user = await GetUserByEmailAsync(email);
        //    if (user == null) return false;

        //    // Example password checking (consider hashing!)
        //    return user.Password == password;
        //}
    }
}
