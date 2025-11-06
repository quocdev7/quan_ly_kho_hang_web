using System.ComponentModel.DataAnnotations;

namespace vnaisoft.common.Models.Users
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