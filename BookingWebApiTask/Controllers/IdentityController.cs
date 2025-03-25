using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BookingWebApiTask.Application.Interfaces;
using BookingWebApiTask.Domain.Entities;
using BookingWebApiTask.Domain.ViewModels;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;

namespace BookingWebApiTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public IdentityController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var result = _mapper.Map<IEnumerable<IdentityUserResult>>(users);
            return result.Any() ? Ok(result) : NotFound("No users found.");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login request.");

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return NotFound("User not found.");

            bool check = await _userManager.CheckPasswordAsync(user, loginDto.PasswordHash);
            if (!check)
                return BadRequest("Invalid password.");

            return Ok(new { message = "Login successful", user.Email });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid registration data.");

            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null)
                return BadRequest("User already exists.");

            var user = _mapper.Map<ApplicationUser>(registerDto);
            user.UserName = registerDto.Email;

            var result = await _userManager.CreateAsync(user, registerDto.PasswordHash);
            if (result.Succeeded)
            {
                return Ok(new { message = "Registration successful", user.Email });
            }

            return BadRequest(result.Errors);
        }
    }
}
