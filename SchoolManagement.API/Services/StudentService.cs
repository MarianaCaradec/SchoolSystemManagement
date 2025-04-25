using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data.Context;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Services
{
    public class StudentService(SchoolSysDBContext context, IUserService userService) : IStudentService
    {
        private readonly SchoolSysDBContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            return await _context.Students
                .Include(st => st.Class)
                .Include(st => st.Attendances)
                .Include(st => st.Grades)
                .Select(st => new Student
                {
                    Id = st.Id,
                    Name = st.Name,
                    Surname = st.Surname,
                    BirthDate = st.BirthDate,
                    Address = st.Address,
                    MobileNumber = st.MobileNumber,
                    User = st.User,
                    Class = st.Class,
                    Attendances = st.Attendances.ToList(),
                    Grades = st.Grades.ToList()
                }).ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            Student student = await _context.Students
                .Include(st => st.User)
                .Include(st => st.Class)
                .Include(st => st.Attendances)
                .Include(st => st.Grades)
                .FirstOrDefaultAsync(st => st.Id == id);

            if (student == null) throw new KeyNotFoundException($"Student with ID {id} not found.");

            return student;
        }

        public async Task<Student> CreateStudentAsync(Student studentToBeCreated)
        {
            Student createdStudent = new Student
            {
                Name = studentToBeCreated.Name,
                Surname = studentToBeCreated.Surname,
                BirthDate = studentToBeCreated.BirthDate,
                Address = studentToBeCreated.Address,
                MobileNumber = studentToBeCreated.MobileNumber,
                UserId = studentToBeCreated.UserId,
                ClassId = studentToBeCreated.ClassId
            };

            if (createdStudent.Id == null || createdStudent.Id == 0)
            {
                Random random = new Random();
                createdStudent.Id = random.Next(1, int.MaxValue);
            }

            _context.Students.Add(createdStudent);
            await _context.SaveChangesAsync();

            return createdStudent;
        }

        public async Task<Student> UpdateStudentAsync(int id, Student studentToBeUpdated)
        {
            Student updatedStudent = await _context.Students
                .Include(st => st.User)
                .Include(st => st.Class)
                .Include(st => st.Attendances)
                .Include(st => st.Grades)
                .FirstOrDefaultAsync(st => st.Id == id);

            if (updatedStudent == null) throw new KeyNotFoundException($"Student with ID {id} not found.");

            await _userService.UpdateUserAsync(updatedStudent.UserId, studentToBeUpdated.User);

            if(studentToBeUpdated.User?.Role == "Admin")
            {
                updatedStudent.Name = studentToBeUpdated.Name;
                updatedStudent.Surname = studentToBeUpdated.Surname;
                updatedStudent.BirthDate = studentToBeUpdated.BirthDate;
                updatedStudent.Address = studentToBeUpdated.Address;
                updatedStudent.MobileNumber = studentToBeUpdated.MobileNumber;
                updatedStudent.ClassId = studentToBeUpdated.ClassId;
                updatedStudent.Attendances = studentToBeUpdated.Attendances ?? updatedStudent.Attendances;
                updatedStudent.Grades = studentToBeUpdated.Grades ?? updatedStudent.Grades;
            } else if(studentToBeUpdated.User?.Role == "Student")
            {
                updatedStudent.Name = studentToBeUpdated.Name;
                updatedStudent.Surname = studentToBeUpdated.Surname;
                updatedStudent.BirthDate = studentToBeUpdated.BirthDate;
                updatedStudent.Address = studentToBeUpdated.Address;
                updatedStudent.MobileNumber = studentToBeUpdated.MobileNumber;
            }

            _context.Students.Update(updatedStudent);
            await _context.SaveChangesAsync();

            return updatedStudent;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            Student studentToBeDeleted = await _context.Students.FindAsync(id);

            if (studentToBeDeleted == null) throw new KeyNotFoundException($"Student with ID {id} not found");

            _context.Students.Remove(studentToBeDeleted);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
