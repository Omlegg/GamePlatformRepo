using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePlatformRepo.Core.Models;
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
        public DateTime DateOfPublication{get;set;}

    }
}