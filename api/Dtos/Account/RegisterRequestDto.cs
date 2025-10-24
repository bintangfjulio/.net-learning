using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
    public class RegisterRequestDto
    {
        [Required]
        [MinLength(4, ErrorMessage = "User name must be at least 4 characters long")]
        [MaxLength(12, ErrorMessage = "User name must be at most 12 characters long")]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(5, ErrorMessage = "Email must be at least 5 characters long")]
        [MaxLength(30, ErrorMessage = "Email must be at most 30 characters long")]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}