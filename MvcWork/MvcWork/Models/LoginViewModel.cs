using System.ComponentModel.DataAnnotations;

namespace MvcWork.Models
{
    public class LoginViewModel
    {
     
        [StringLength(50, ErrorMessage = "MailAddress maksimum 50 karakterden oluşabilir.")]
        public string? MailAddress { get; set; }

        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter olabilir.")]
        [MaxLength(16, ErrorMessage = "Şifre maksimum 16 karakterden oluşabilir.")]
        public string? Password { get; set; }
    }
}
