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

            IQueryable<User> query = _context.Users.Include(u => u.Email).Include(u => u.Role);

            if(userRole == "Admin")
            {
                query = query
                    .Include(u => u.Password)
                    .Include(u => u.Teacher)
                    .Include(u => u.Student);
            }

            return await query.Select(u => new User
            {
                Email = u.Email,
                Password = userRole == "Admin" ? u.Password : null,
                Role = u.Role,
                Teacher = userRole == "Admin" ? u.Teacher : null,
                Student = userRole == "Admin" ? u.Student : null,
            }).ToListAsync(); ;
        }

        public async Task<User> GetUserByEmailAsync(string email, int userId)
        {
            string userRole = await GetUserRole(userId);

            IQueryable<User> query = _context.Users.Include(u => u.Email).Include(u => u.Role);

            if(userRole == "Admin")
            {
                query = query
                    .Include(u => u.Password)
                    .Include(u => u.Teacher)
                    .Include(u => u.Student);
            }

            User user = await query.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) throw new KeyNotFoundException($"User with email {email} not found.");

            return user;
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

            User user = await _context.Users
                .Include(u => u.Email)
                .Include(u => u.Password)
                .Include(u => u.Role)
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) throw new KeyNotFoundException($"User with ID {id} not found");

            if (userRole != "Admin")
            {
                user.Email = userToBeUpdated.Email;
                user.Password = userToBeUpdated.Password;
            }
            else
            {
                user.Email = userToBeUpdated.Email;
                user.Password = userToBeUpdated.Password;
                user.Role = userToBeUpdated.Role;
            }

            _context.Update(user);   
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUserAsync(int id, int userId)
        {
            string userRole = await GetUserRole(userId);

            if (userRole != "Admin")
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
