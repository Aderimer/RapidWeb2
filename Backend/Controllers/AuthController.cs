using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Backend.Data;
using Backend.Models;

namespace Backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly DataContext _dataContext;

        public AuthController(AuthService authService, DataContext dataContext)
        {
            _authService = authService;
            _dataContext = dataContext;
        }



        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            if (await _authService.RegisterUserAsync(request.Username, request.Email, request.Password, request.Role))
            {
                return Ok("Registration successfull");
            }
            return BadRequest("Email already in use. Please use a different one or contact administrator.");
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            if (await _authService.ValidateUserAsync(request.Email, request.Password))
            {
                var user = await _authService.GetUserByEmailAsync(request.Email);
                var token = _authService.GenerateToken(user);
                var userId = user.Id.ToString();
                return Ok(new { Token = token, Email = request.Email, UserId = userId, Role = user.Role });
            }
            return Unauthorized("Invalid email or password");
        }


        [HttpDelete("userId")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _dataContext.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("No user find with that ID");
            }

            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();
            return Ok("User deleted.");
        }
    }
}