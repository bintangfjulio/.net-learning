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
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        [MaxLength(10, ErrorMessage = "Username must be at most 10 characters long")]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(3, ErrorMessage = "Email must be at least 3 characters long")]
        [MaxLength(20, ErrorMessage = "Email must be at most 20 characters long")]
        public string? Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [MaxLength(12, ErrorMessage = "Password must be at most 12 characters long")]
        public string? Password { get; set; }
    }
}