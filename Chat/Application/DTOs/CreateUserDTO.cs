using System.ComponentModel.DataAnnotations;

namespace Chat.Application.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        [MaxLength(40)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(15)]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required]
        public bool IsPublicProfile { get; set; }
    }
}
