using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatformRepo.Exceptions
{
    public class NotFoundException(string key, string? message = null) : Exception(message){
        public string Key { get; set; } = key;
    }
}