using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController(ISubjectService subjectService) : ControllerBase
    {
        private readonly ISubjectService _subjectService = subjectService;

        // GET: api/<SubjectController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectResponseDto>>> GetAllSubjects(int userId)
        {
            IEnumerable<SubjectResponseDto> subjects = await _subjectService.GetSubjectsAsync(userId);

            if (subjects == null || !subjects.Any()) return NoContent();

            return Ok(subjects);
        }

        // GET api/<SubjectController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectResponseDto>> GetSubject(int id, int userId)
        {
            SubjectResponseDto subject = await _subjectService.GetSubjectByIdAsync(id, userId);

            return Ok(subject);
        }

        // POST api/<SubjectController>
        [HttpPost]
        public async Task<ActionResult<SubjectInputDto>> PostSubject(SubjectInputDto subjectToBeCreated, int userId)
        {
            SubjectInputDto createdSubject = await _subjectService.CreateSubjectAsync(subjectToBeCreated, userId);

            return CreatedAtAction("GetSubject", new { id = createdSubject.Id }, createdSubject);
        }

        // PUT api/<SubjectController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<SubjectInputDto>> PutSubject(int id, [FromBody] SubjectInputDto subjectToBeUpdated, int userId)
        {
            SubjectInputDto updatedSubject = await _subjectService.UpdateSubjectAsync(id, subjectToBeUpdated, userId);
            
            return Ok(updatedSubject);
        }

        // DELETE api/<SubjectController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id, int userId)
        {
            await _subjectService.DeleteSubjectAsync(id, userId);

            return NoContent();
        }
    }
}
