using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Services
{
    public class UserService(SchoolSysDBContext context) : IUserService
    {
        private readonly SchoolSysDBContext _context = context;

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public string GetUserRole(int id)
        {
            User user = _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .FirstOrDefault(u => u.Id == id);

            if (user == null) throw new KeyNotFoundException($"User with ID {id} not found");

            return user.Role;
        }

        public async Task<User> CreateUserAsync(User userToBeCreated)
        {
            User createdUser = new User
            {
                Email = userToBeCreated.Email,
                Password = userToBeCreated.Password,
                Role = userToBeCreated.Role
            };

            if(createdUser.Id == null || createdUser.Id == 0)
            {
                Random random = new Random();
                createdUser.Id = random.Next(1, int.MaxValue);
            }

            _context.Users.Add(userToBeCreated);
            await _context.SaveChangesAsync();

            return createdUser;
        }

        public async Task<User> UpdateUserAsync(int id, User userToBeUpdated)
        {
            User updatedUser = await _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (updatedUser == null) throw new KeyNotFoundException($"User with ID {id} not found");

            updatedUser.Email = userToBeUpdated.Email;
            updatedUser.Password = userToBeUpdated.Password;

            if(userToBeUpdated.Teacher != null && updatedUser.Role == "Teacher")
            {
                updatedUser.Teacher.Name = userToBeUpdated.Teacher.Name;
                updatedUser.Teacher.Surname = userToBeUpdated.Teacher.Surname;
                updatedUser.Teacher.BirthDate = userToBeUpdated.Teacher.BirthDate;
                updatedUser.Teacher.Address = userToBeUpdated.Teacher.Address;
                updatedUser.Teacher.MobileNumber = userToBeUpdated.Teacher.MobileNumber;
            }
            else if (userToBeUpdated.Student != null || updatedUser.Role == "Student")
            {
                updatedUser.Student.Name = userToBeUpdated.Student.Name;
                updatedUser.Student.Surname = userToBeUpdated.Student.Surname;
                updatedUser.Student.BirthDate = userToBeUpdated.Student.BirthDate;
                updatedUser.Student.Address = userToBeUpdated.Student.Address;
                updatedUser.Student.MobileNumber = userToBeUpdated.Student.MobileNumber;
            }
            else if (userToBeUpdated.Role == "Admin")
            {
                updatedUser.Role = userToBeUpdated.Role;

                if(userToBeUpdated.Teacher != null)
                {
                    updatedUser.Teacher.Name = userToBeUpdated.Teacher.Name;
                    updatedUser.Teacher.Surname = userToBeUpdated.Teacher.Surname;
                    updatedUser.Teacher.BirthDate = userToBeUpdated.Teacher.BirthDate;
                    updatedUser.Teacher.Address = userToBeUpdated.Teacher.Address;
                    updatedUser.Teacher.MobileNumber = userToBeUpdated.Teacher.MobileNumber;
                }

                if(userToBeUpdated.Student != null)
                {
                    updatedUser.Student.Name = userToBeUpdated.Student.Name;
                    updatedUser.Student.Surname = userToBeUpdated.Student.Surname;
                    updatedUser.Student.BirthDate = userToBeUpdated.Student.BirthDate;
                    updatedUser.Student.Address = userToBeUpdated.Student.Address;
                    updatedUser.Student.MobileNumber = userToBeUpdated.Student.MobileNumber;
                }
                
            }

            _context.Update(updatedUser);   
            await _context.SaveChangesAsync();

            return updatedUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            User userToBeDeleted = await _context.Users.FindAsync(id);

            if (userToBeDeleted == null) throw new KeyNotFoundException($"User with ID {id} not found");
            
            _context.Users.Remove(userToBeDeleted);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null) return false;

            // Example password checking (consider hashing!)
            return user.Password == password;
        }
    }
}
