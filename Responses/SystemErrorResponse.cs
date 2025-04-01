using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePlatformRepo.Responses.Base;

namespace GamePlatformRepo.Responses
{
    public class SystemErrorResponse (string? message = null) : BaseResponse(message) { }
}