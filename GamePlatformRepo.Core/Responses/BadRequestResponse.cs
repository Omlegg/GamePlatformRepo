using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePlatformRepo.Responses.Base;

namespace GamePlatformRepo.Responses
{
    public class BadRequestResponse(string? message = null, string? paramName = null) : BaseResponse(message)
    {
        public string? ParamName { get; set; } = paramName;
    }
}