//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using WatchmanDevotional.Data;
//using WatchmanDevotional.DTOs;
//using WatchmanDevotional.Helpers;
//using WatchmanDevotional.Models;
//using WatchmanDevotional.Services;
//using Microsoft.EntityFrameworkCore;

//namespace WatchmanDevotional.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]

//    public class AuthController : ControllerBase
//    {
//        private readonly WatchmanDevotionDbContext _context;
//        private readonly AuthService _authService;
//        private readonly IConfiguration _config;

//        public AuthController(WatchmanDevotionDbContext context, AuthService authService, IConfiguration config)
//        {
//            _context = context;
//            _authService = authService;
//            _config = config;
//        }
//        //migration needed for user table before testing this endpoint
//        [HttpPost("login")]
//        public async Task<IActionResult> Login(LoginDto dto)
//        {
//            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

//            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
//                return Unauthorized("Invalid credentials");

//            var token = _authService.CreateToken(user, _config);

//            return Ok(new { token, user.FullName, user.Role });
//        }

//        [Authorize(Roles = "SuperAdmin")]
//        [HttpPost("register-admin")]
//        public async Task<IActionResult> RegisterAdmin(RegisterDto dto)
//        {
//            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
//            if (exists) return BadRequest("Email already exists");

//            var user = new User
//            {
//                FullName = dto.FullName,
//                Email = dto.Email,
//                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
//                Role = UserRoles.Admin
//            };

//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();

//            return Ok("Admin created successfully");
//        }
//        [Authorize(Roles = "SuperAdmin")]
//        [HttpGet("admins")]
//        public async Task<IActionResult> GetAllAdmins()
//        {
//            var admins = await _context.Users
//                .Where(u => u.Role == "Admin")
//                .Select(u => new AdminUserDto
//                {
//                    Id = u.Id,
//                    FullName = u.FullName,
//                    Email = u.Email,
//                    IsActive = u.IsActive
//                })
//                .ToListAsync();

//            return Ok(admins);
//        }

//        [Authorize(Roles = "SuperAdmin")]
//        [HttpPut("admins/{id}/toggle-status")]
//        public async Task<IActionResult> ToggleAdminStatus(Guid id)
//        {
//            var admin = await _context.Users.FindAsync(id);
//            if (admin == null || admin.Role != "Admin")
//            {
//                return NotFound("Admin account not found.");
//            }

//            // Toggle active status flip
//            admin.IsActive = !admin.IsActive;
//            await _context.SaveChangesAsync();

//            return Ok(new { message = $"Admin status updated successfully to: {(admin.IsActive ? "Active" : "Deactivated")}" });
//        }
//    }
//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WatchmanDevotional.Data;
using WatchmanDevotional.DTOs;
using WatchmanDevotional.Helpers;
using WatchmanDevotional.Models;
using WatchmanDevotional.Services;
using Microsoft.EntityFrameworkCore;

namespace WatchmanDevotional.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly WatchmanDevotionDbContext _context;
        private readonly AuthService _authService;
        private readonly IConfiguration _config;

        public AuthController(WatchmanDevotionDbContext context, AuthService authService, IConfiguration config)
        {
            _context = context;
            _authService = authService;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _authService.CreateToken(user, _config);

            return Ok(new { token, user.FullName, user.Role });
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterDto dto)
        {
            // Automatic validation occurs via [ApiController] attribute checking RegisterDto rules
            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists) return BadRequest("Email already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber, // 🌟 Saves phone number mapping entry
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRoles.Admin
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Admin created successfully");
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("admins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _context.Users
                .Where(u => u.Role == "Admin")
                .Select(u => new AdminUserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    IsActive = u.IsActive
                })
                .ToListAsync();

            return Ok(admins);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("admins/{id}/toggle-status")]
        public async Task<IActionResult> ToggleAdminStatus(Guid id)
        {
            var admin = await _context.Users.FindAsync(id);
            if (admin == null || admin.Role != "Admin")
            {
                return NotFound("Admin account not found.");
            }

            admin.IsActive = !admin.IsActive;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Admin status updated successfully to: {(admin.IsActive ? "Active" : "Deactivated")}" });
        }
    }
}