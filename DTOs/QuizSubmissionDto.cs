//using System.ComponentModel.DataAnnotations;

//namespace WatchmanDevotional.DTOs
//{
//    public class QuizSubmissionDto
//    {
//        [Required]
//        [StringLength(100, MinimumLength = 3)]
//        public string FullName { get; set; } = string.Empty;

//        [Required]
//        [Phone]
//        public string PhoneNumber { get; set; } = string.Empty;

//        [Required]
//        public string Answer { get; set; } = string.Empty;
//    }
//}


using System.ComponentModel.DataAnnotations;

namespace WatchmanDevotional.DTOs
{
    public class QuizSubmissionDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        // 🌟 REPLACED [Phone] with a strict Regex for 11-digit Nigerian mobile numbers
        [RegularExpression(@"^(070|080|081|090|091|071|082)\d{8}$",
            ErrorMessage = "Phone number must be exactly 11 digits and a valid Nigerian mobile number starting with 070, 080, 081, 090, or 091.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Answer { get; set; } = string.Empty;
    }
}