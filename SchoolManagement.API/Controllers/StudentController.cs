using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController(IStudentService studentService) : ControllerBase
    {
        private readonly IStudentService _studentService = studentService;

        // GET: api/<StudentController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetAllStudents(int userId)
        {
            IEnumerable<StudentResponseDto> students = await _studentService.GetStudentsAsync(userId);

            if (students == null || !students.Any()) return NoContent();

            return Ok(students);
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentResponseDto>> GetStudent(int id, int userId)
        {
            StudentResponseDto student = await _studentService.GetStudentByIdAsync(id, userId);

            return Ok(student);
        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<ActionResult<StudentInputDto>> PostStudent(StudentInputDto studentToBeCreated, int userId)
        {
            StudentInputDto createdStudent = await _studentService.CreateStudentAsync(studentToBeCreated, userId);

            return CreatedAtAction("GetStudent", new { id = createdStudent.Id }, createdStudent);
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<StudentInputDto>> PutStudent(int id, StudentInputDto studentToBeUpdated, int userId)
        {
            StudentInputDto updatedStudent = await _studentService.UpdateStudentAsync(id, studentToBeUpdated, userId);

            return Ok(updatedStudent);
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteStudent(int id, int userId)
        {
            await _studentService.DeleteStudentAsync(id, userId);
            
            return NoContent();
        }
    }
}
