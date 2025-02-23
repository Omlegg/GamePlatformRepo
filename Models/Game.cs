using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatformRepo.Models
{
    public class Game
    {
        public string Name {get;set;} = "";
        public int Views {get;set;}
        public string? Description {get;set;}
        public int Id {get;set;}
        public decimal Price {get;set;}

    }
}