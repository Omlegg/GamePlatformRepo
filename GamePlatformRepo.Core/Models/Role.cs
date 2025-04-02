using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatformRepo.Core.Models
{
    public class Role
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<UsersRoles> UsersRoles { get; set; }

    }
}