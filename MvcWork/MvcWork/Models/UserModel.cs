using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MvcWork.Models
{
    public class UserModel
    {

        public int UserId { get; set; }

        public string? FirstName { get; set; }
      
        public string? LastName { get; set; }
     
        public string? MailAddress { get; set; }
    
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; } = true;

    }
}