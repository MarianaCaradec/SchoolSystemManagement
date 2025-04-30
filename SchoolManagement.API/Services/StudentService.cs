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

        public async Task<IEnumerable<Student>> GetStudentsAsync(int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            IQueryable<Student> query = _context.Students
                    .Include(s => s.User)
                    .Include(st => st.Class)
                    .Include(st => st.Attendances)
                    .Include(st => st.Grades);

            if (userRole == "Teacher")
            {
                query = query.Where(s => s.Class.Teachers.Any(t => t.Id == userId));
            }
            
            if(userRole == "Student")
            {
                query = query.Where(s => s.Class.Students.Any(s => s.Id == userId));
            } 

            return await query.Select(st => new Student
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
            }).ToListAsync(); ;
        }

        public async Task<Student> GetStudentByIdAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            IQueryable<Student> query = _context.Students
                .Include(st => st.User)
                .Include(st => st.Class)
                .Include(st => st.Attendances)
                .Include(st => st.Grades);

            if (userRole == "Teacher")
            {
                query = query.Where(st => st.Class.Teachers.Any(t => t.Id == userId));
            }
            
            if(userRole == "Student")
            {
                if (id != userId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to access this student.");
                }

                query = query.Where(st => st.Id == userId);
            }

            Student student = await query.FirstOrDefaultAsync(st => st.Id == id);

            if (student == null) throw new KeyNotFoundException($"Student with ID {id} not found.");

            return student;
        }

        public async Task<Student> CreateStudentAsync(Student studentToBeCreated, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if(userRole != "Admin")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

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

        public async Task<Student> UpdateStudentAsync(int id, Student studentToBeUpdated, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if(userRole == "Teacher")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Student student = await _context.Students
                .Include(st => st.User)
                .Include(st => st.Class)
                .Include(st => st.Attendances)
                .Include(st => st.Grades)
                .FirstOrDefaultAsync(st => st.Id == id);

            if (student == null) throw new KeyNotFoundException($"Student with ID {id} not found.");

            if(userRole == "Student")
            {
                if(id != userId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to do this action.");

                }

                student.Name = studentToBeUpdated.Name;
                student.Surname = studentToBeUpdated.Surname;
                student.BirthDate = studentToBeUpdated.BirthDate;
                student.Address = studentToBeUpdated.Address;
                student.MobileNumber = studentToBeUpdated.MobileNumber;
            } 

             student.Name = studentToBeUpdated.Name;
             student.Surname = studentToBeUpdated.Surname;
             student.BirthDate = studentToBeUpdated.BirthDate;
             student.Address = studentToBeUpdated.Address;
             student.MobileNumber = studentToBeUpdated.MobileNumber;
             student.ClassId = studentToBeUpdated.ClassId;
             student.Attendances = studentToBeUpdated.Attendances ?? student.Attendances;
             student.Grades = studentToBeUpdated.Grades ?? student.Grades;

            _context.Students.Update(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<bool> DeleteStudentAsync(int id, int userId)
        {
            string userRole = await _userService.GetUserRole(userId);

            if(userRole != "Admin")
            {
                throw new UnauthorizedAccessException("You are not authorized to do this action.");
            }

            Student studentToBeDeleted = await _context.Students.FindAsync(id);

            if (studentToBeDeleted == null) throw new KeyNotFoundException($"Student with ID {id} not found");

            _context.Students.Remove(studentToBeDeleted);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
