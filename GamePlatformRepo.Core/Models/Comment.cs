using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatformRepo.Models
{
    public class Comment
    {
        public string Title {get;set;} = "No Title";

        [Required]
        public string Description {get;set;}
        public string Author {get;set;} = "Unknown";

        public int GameId {get;set;}
        public int Id {get;set;}
    }
};