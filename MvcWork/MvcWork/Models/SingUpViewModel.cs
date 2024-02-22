using System.ComponentModel.DataAnnotations;

namespace MvcWork.Models
{
    public class SingUpViewModel
    {
        [Required(ErrorMessage = "Ad alanı gereklidir")]
        [StringLength(20, ErrorMessage = "Ad, maksimum 20 karakterden oluşabilir.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyadı alanı gereklidir")]
        [StringLength(20, ErrorMessage = "Soyadı maksimum 20 karakterden oluşabilir.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Mail adresi alanı gereklidir")]
        [StringLength(50, ErrorMessage = "MailAddress maksimum 50 karakterden oluşabilir.")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@gmail\.com$", ErrorMessage = "Geçerli bir gmail adresi olmalıdır")]
        public string MailAddress { get; set; }

        [Required(ErrorMessage = "Şifre alanı gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter olabilir.")]
        [MaxLength(16, ErrorMessage = "Şifre maksimum 16 karakter olabilir.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakterden oluşmalıdır")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı alanı gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Yeniden Şifre en az 8 karakter olabilir.")]
        [MaxLength(16, ErrorMessage = "Yeniden Şifre en fazla 16 karakter olabilir.")]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}
