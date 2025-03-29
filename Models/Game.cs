using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePlatformRepo.Models;

namespace GamePlatformRepo.Models
{
    public class Game
    {
        public int Id {get;set;}
        public string Name {get;set;} = "";
        public int Views {get;set;} = 0;
        public string? Description {get;set;}
        public decimal Price {get;set;}
        public string Creator {get;set;} = "Unknown";
        public DateTime DateOfPublication{get;set;}

    }
}