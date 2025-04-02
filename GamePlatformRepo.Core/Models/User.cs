using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatformRepo.Core.Models
{
    public class User
    {
        public int Id { get; set; }


        public string Name { get; set; }

        public string Password { get; set; }

        public DateTime DateOfCreation {get;set;} = DateTime.Now;
        public string Mail {get;set;}
    }
}