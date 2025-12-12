using System.ComponentModel.DataAnnotations;

namespace quan_ly_kho.common.Models.Users
{
    public class AuthenticateModel
    {
        public string capcha { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}