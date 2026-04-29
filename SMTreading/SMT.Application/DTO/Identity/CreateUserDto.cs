using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.DTO.Identity
{
    public class CreateUserRequest
    {
        //public Guid TenantId { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string AvatarUrl { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
