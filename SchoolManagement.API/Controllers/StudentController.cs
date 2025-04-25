﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            IEnumerable<Student> students = await _studentService.GetStudentsAsync();

            if (students == null || !students.Any()) return NoContent();

            return Ok(students);
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            Student student = await _studentService.GetStudentByIdAsync(id);

            return Ok(student);
        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student studentToBeCreated)
        {
            Student createdStudent = await _studentService.CreateStudentAsync(studentToBeCreated);

            return CreatedAtAction("GetStudent", new { id = createdStudent.Id }, createdStudent);
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> PutStudent(int id, Student studentToBeUpdated)
        {
            Student updatedStudent = await _studentService.UpdateStudentAsync(id, studentToBeUpdated);

            return Ok(updatedStudent);
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteStudent(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            
            return NoContent();
        }
    }
}
