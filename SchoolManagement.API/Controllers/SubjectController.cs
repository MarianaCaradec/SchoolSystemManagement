using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<Subject>>> GetAllSubjects()
        {
            IEnumerable<Subject> subjects = await _subjectService.GetSubjectsAsync();

            if (subjects == null || !subjects.Any()) return NoContent();

            return Ok(subjects);
        }

        // GET api/<SubjectController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubject(int id)
        {
            Subject subject = await _subjectService.GetSubjectByIdAsync(id);

            return Ok(subject);
        }

        // POST api/<SubjectController>
        [HttpPost]
        public async Task<ActionResult<Subject>> PostSubject(Subject subjectToBeCreated)
        {
            Subject createdSubject = await _subjectService.CreateSubjectAsync(subjectToBeCreated);

            return CreatedAtAction("GetSubject", new { id = createdSubject.Id }, createdSubject);
        }

        // PUT api/<SubjectController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Subject>> PutSubject(int id, Subject subjectToBeUpdated)
        {
            Subject updatedSubject = await _subjectService.UpdateSubjectAsync(id, subjectToBeUpdated);
            
            return Ok(updatedSubject);
        }

        // DELETE api/<SubjectController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            await _subjectService.DeleteSubjectAsync(id);

            return NoContent();
        }
    }
}
