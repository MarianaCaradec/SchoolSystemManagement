using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs;
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
        public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetAllAttendances(int userId)
        {
            IEnumerable<AttendanceDto> attendances = await _attendanceService.GetAttendancesAsync(userId);

            if (attendances == null || !attendances.Any()) return NoContent();

            return Ok(attendances);
        }

        // GET api/<AttendanceController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AttendanceResponseDto>> GetAttendance(int id, int userId)
        {
            AttendanceResponseDto attendance = await _attendanceService.GetAttendanceByIdAsync(id, userId);

            return Ok(attendance);
        }

        // POST api/<AttendanceController>
        [HttpPost]
        public async Task<ActionResult<AttendanceDto>> PostAttendance(AttendanceDto attendanceToBeCreated, int userId)
        {
            AttendanceDto createdAttendance = await _attendanceService.CreateAttendanceAsync(attendanceToBeCreated, userId);
            
            return CreatedAtAction("GetAttendance", new { id = createdAttendance.Id }, createdAttendance);
        }

        // PUT api/<AttendanceController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AttendanceDto>> PutAttendance(int id, AttendanceDto attendanceToBeUpdated, int userId)
        {
            AttendanceDto updatedAttendance = await _attendanceService.UpdateAttendanceAsync(id, attendanceToBeUpdated, userId);
            
            return Ok(updatedAttendance);
        }

        // DELETE api/<AttendanceController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id, int userId)
        {
            await _attendanceService.DeleteAttendanceAsync(id, userId);

            return NoContent();
        }
    }
}
