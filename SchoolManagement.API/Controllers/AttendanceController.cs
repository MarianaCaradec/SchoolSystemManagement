using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController(IAttendanceService attendanceService) : ControllerBase
    {

        private readonly IAttendanceService _attendanceService = attendanceService;

        // GET: api/<AttendanceController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAllAttendances()
        {
            IEnumerable<Attendance> attendances = await _attendanceService.GetAttendancesAsync();

            if (attendances == null || !attendances.Any()) return NoContent();

            return Ok(attendances);
        }

        // GET api/<AttendanceController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendance(int id)
        {
            Attendance attendance = await _attendanceService.GetAttendanceByIdAsync(id);

            return Ok(attendance);
        }

        // POST api/<AttendanceController>
        [HttpPost]
        public async Task<ActionResult<Attendance>> PostAttendance(Attendance attendanceToBeCreated)
        {
            Attendance createdAttendance = await _attendanceService.CreateAttendanceAsync(attendanceToBeCreated, attendanceToBeCreated?.StudentId, attendanceToBeCreated?.TeacherId);
            
            return CreatedAtAction("GetAttendance", new { id = createdAttendance.Id }, createdAttendance);
        }

        // PUT api/<AttendanceController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Attendance>> PutAttendance(int id, Attendance attendanceToBeUpdated)
        {
            Attendance updatedAttendance = await _attendanceService.UpdateAttendanceAsync(id, attendanceToBeUpdated, attendanceToBeUpdated?.StudentId, attendanceToBeUpdated?.TeacherId);
            
            return Ok(updatedAttendance);
        }

        // DELETE api/<AttendanceController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            await _attendanceService.DeleteAttendanceAsync(id);

            return NoContent();
        }
    }
}
