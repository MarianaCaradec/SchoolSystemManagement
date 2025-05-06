using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("Authenticate")]
        public async Task<ActionResult<string>> Authenticate([FromBody] AuthReq req)
        {
            return await _authService.AuthenticateAsync(req.Email, req.Password, req.UserId);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] Auth authUser, [FromQuery] int userId)
        {
            return await _authService.RegisterAsync(authUser, userId);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthDto>> Login([FromBody] LoginReq req)
        {
            return await _authService.LoginAsync(req.Email, req.Password);
        }   
    }
}
