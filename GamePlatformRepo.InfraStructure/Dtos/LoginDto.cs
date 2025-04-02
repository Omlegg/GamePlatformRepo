using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatformRepo.InfraStructure.Dtos
{
    public class LoginDto
    {
        public string? ReturnUrl { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
    }
}