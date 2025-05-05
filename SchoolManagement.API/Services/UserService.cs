using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using static SchoolManagement.API.Models.User;

namespace SchoolManagement.API.Services
{
    public class UserService(SchoolSysDBContext context, IPasswordHasher<User> passwordHasher) : IUserService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

        public async Task<IEnumerable<User>> GetUsersAsync(int userId)
        {
            UserRole userRole = await GetUserRole(userId);

            IQueryable<User> query = _context.Users;

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

            IQueryable<User> query = _context.Users;

            User user = await query.Select(u => new User
            {
                Email = u.Email,
                Role = u.Role,
                Password = userRole == UserRole.Admin ? u.Password : null,
                Teacher = userRole == UserRole.Admin ? u.Teacher : null,
                Student = userRole == UserRole.Admin ? u.Student : null
            }).FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) throw new KeyNotFoundException($"User with email {email} not found.");

            if (userRole != UserRole.Admin && userId != user.Id)
            {
                throw new UnauthorizedAccessException("You are not authorized to view this user.");
            }

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

        private string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public async Task<User> CreateUserAsync(User userToBeCreated, int userId)
        {
            UserRole creatorRole = await GetUserRole(userId);


            if (creatorRole == null)
            {
                if (userToBeCreated.Role != UserRole.Student)
                {
                    throw new UnauthorizedAccessException("Only student accounts can be created without authentication.");
                }
            }
            else
            {
                if(creatorRole == UserRole.Student)
                {
                    throw new UnauthorizedAccessException("Students are not authorized to create an user.");
                }

                if ((userToBeCreated.Role == UserRole.Admin || userToBeCreated.Role == UserRole.Teacher) && creatorRole != UserRole.Admin)
                {
                    throw new UnauthorizedAccessException($"Only Admin users can assign {userToBeCreated.Role} role.");
                }
            }

            if (!Enum.TryParse(userToBeCreated.Role.ToString(), true, out UserRole inputRole))
            {
                throw new ArgumentException($"Invalid role '{userToBeCreated.Role}'. Allowed roles are: Admin and Teacher for an Admin user, or Student for anyone.");
            }

            string hashedPassword = HashPassword(userToBeCreated.Password);

            User createdUser = new User
            {
                Email = userToBeCreated.Email,
                Password = hashedPassword,
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

            if (!Enum.TryParse(userToBeUpdated.Role.ToString(), true, out UserRole inputRole))
            {
                throw new ArgumentException($"Invalid role '{userToBeUpdated.Role}'. Allowed roles are: Admin and Teacher for an Admin user, or Student for anyone.");
            }

            string hashedPassword = HashPassword(userToBeUpdated.Password);

            if (userRole != UserRole.Admin)
            {
                if(id != userId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to do this action.");
                } else
                {
                    user.Email = userToBeUpdated.Email;
                    user.Password = hashedPassword;
                }
            }
            else
            {
                user.Email = userToBeUpdated.Email;
                user.Password = userToBeUpdated.Password;
                user.Role = inputRole;
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

        public async Task<bool> AuthenticateAsync(string email, string password, int userId)
        {
            var user = await GetUserByEmailAsync(email, userId);

            if (user == null) return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            return result == PasswordVerificationResult.Success;
        }
    }
}
