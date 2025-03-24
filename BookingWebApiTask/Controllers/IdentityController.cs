using AutoMapper;
using BookingWebApiTask.Application.Interfaces;
using BookingWebApiTask.Domain.Entities;
using BookingWebApiTask.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingWebApiTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public IdentityController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.ApplicationUser.GetAllAsync();
            return users.Any() ? Ok(users) : NotFound("No users found.");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest("Invalid login request.");

            var users = await _unitOfWork.ApplicationUser.GetAllAsync(filter:x => x.Email == loginDto.Email);
            var user = users.FirstOrDefault();

            if (user == null)
                return NotFound("User not found.");

            var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);

            if (passwordVerification == PasswordVerificationResult.Failed)
                return BadRequest("Invalid password.");

            return Ok(new { message = "Login successful", user.Email });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid registration data.");

            bool userExists = (await _unitOfWork.ApplicationUser.GetAllAsync(filter: x => x.Email == registerDto.Email)).Any();
            if (userExists)
                return BadRequest("User already exists.");

            ApplicationUser user = _mapper.Map<ApplicationUser>(registerDto);
            user.Password = _passwordHasher.HashPassword(user, registerDto.Password);

            await _unitOfWork.ApplicationUser.AddAsync(user);
            int rows = await _unitOfWork.SaveChangesAsync();

            if (rows > 0)
            {
                return Ok(new { message = "Registration successful", user.Email });
            }

            return BadRequest("Registration failed.");
        }
    }
}
