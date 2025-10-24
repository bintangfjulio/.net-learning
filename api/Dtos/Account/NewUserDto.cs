using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
    public class NewUserDto
    {
        public String UserName { get; set; } = String.Empty;
        public String Email { get; set; } = String.Empty;
        public String Token { get; set; } = String.Empty;
    }
}