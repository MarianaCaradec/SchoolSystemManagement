using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using static SchoolManagement.API.Models.User;

namespace SchoolManagement.API.Services
{
    public class UserService(SchoolSysDBContext context) : IUserService
    {
        private readonly SchoolSysDBContext _context = context;

        public async Task<IEnumerable<User>> GetUsersAsync(int userId)
        {
            UserRole userRole = await GetUserRole(userId);

            IQueryable<User> query = _context.Users.Include(u => u.Email).Include(u => u.Role);

            if(userRole == UserRole.Admin)
            {
                query = query
                    .Include(u => u.Password)
                    .Include(u => u.Teacher)
                    .Include(u => u.Student);
            }

            return await query.Select(u => new User
            {
                Email = u.Email,
                Password = userRole == UserRole.Admin ? u.Password : null,
                Role = u.Role,
                Teacher = userRole == UserRole.Admin ? u.Teacher : null,
                Student = userRole == UserRole.Admin ? u.Student : null,
            }).ToListAsync(); ;
        }

        public async Task<User> GetUserByEmailAsync(string email, int userId)
        {
            UserRole userRole = await GetUserRole(userId);

            IQueryable<User> query = _context.Users.Include(u => u.Email).Include(u => u.Role);

            if(userRole == UserRole.Admin)
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

        public async Task<UserRole> GetUserRole(int id)
        {
            User user = await _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) throw new KeyNotFoundException($"User with ID {id} not found");

            return user.Role;
        }

        public async Task<User> CreateUserAsync(User userToBeCreated, int userId)
        {
            UserRole creatorRole = await GetUserRole(userId);

            if(userToBeCreated.Role == UserRole.Admin && creatorRole != UserRole.Admin)
            {
                throw new UnauthorizedAccessException("Only Admin users can assign the Admin role to an user");
            }

            if(userToBeCreated.Role == UserRole.Teacher && creatorRole != UserRole.Admin)
            {
                throw new UnauthorizedAccessException("Only Admin users can assign the Teacher role to an user");
            }

            if(creatorRole == null)
            {
                userToBeCreated.Role = UserRole.Student;
            }

            if (!Enum.TryParse(userToBeCreated.Role.ToString(), true, out UserRole inputRole))
            {
                throw new ArgumentException($"Invalid role '{userToBeCreated.Role}'. Allowed roles are: Admin and Teacher for an Admin user, or Student for anyone.");
            }

            User createdUser = new User
            {
                Email = userToBeCreated.Email,
                Password = userToBeCreated.Password,
                Role = inputRole
            };

            _context.Users.Add(createdUser);
            await _context.SaveChangesAsync();

            return createdUser;
        }

        public async Task<User> UpdateUserAsync(int id, User userToBeUpdated, int userId)
        {
            UserRole userRole = await GetUserRole(userId);

            User user = await _context.Users
                .Include(u => u.Email)
                .Include(u => u.Password)
                .Include(u => u.Role)
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) throw new KeyNotFoundException($"User with ID {id} not found");

            if (userRole != UserRole.Admin)
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
            UserRole userRole = await GetUserRole(userId);

            if (userRole != UserRole.Admin)
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
