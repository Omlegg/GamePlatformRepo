using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatformRepo.Responses.Base
{
    public abstract class BaseResponse(string? mssg  = null)
    {
        public string? Message { get; set; } = mssg;
    }
}