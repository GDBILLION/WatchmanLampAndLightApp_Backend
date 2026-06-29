using WatchmanDevotional.Models;
using WatchmanDevotional.Services;

namespace WatchmanDevotional.Services
{
    public class PasswordService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}


// Inside a one-time setup method
//var superAdmin = new User
//{
//    FullName = "Godwin",
//    Email = "godwin@watchman.org",
//    PasswordHash = _passwordService.HashPassword("Watchman2026"),
//    Role = "SuperAdmin"
//};
// Add to database and Save...