//namespace WatchmanDevotional.DTOs
//{
//    public record RegisterDto(string FullName, string Email, string Password);
//}

using System.ComponentModel.DataAnnotations;

namespace WatchmanDevotional.DTOs
{
    public record RegisterDto(
        [Required] string FullName,
        [Required, EmailAddress] string Email,
        [Required] string Password,
        [Required]
        [RegularExpression(@"^(070|080|081|090|091|071|082)\d{8}$", ErrorMessage = "Phone number must be exactly 11 digits long and a valid Nigerian number starting with 070, 080, 081, 090, or 091.")]
        string PhoneNumber
    );
}