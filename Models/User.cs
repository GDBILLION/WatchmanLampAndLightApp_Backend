namespace WatchmanDevotional.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin";
        public bool IsActive { get; set; } = true;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
