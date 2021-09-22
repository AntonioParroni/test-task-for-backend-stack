using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Models;
using Server.DTO;
using Server.Helper;

#nullable enable

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/sessions/byhour")]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;

        public SessionController(ILogger<SessionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get([FromQuery] string? startTime, [FromQuery] string? endTime)
        {
            if (endTime == null && startTime == null) // no params case, return all
            {
                var beautifulInfo = (List<BySessionHour>)new Strategy(new SessionReturnAll()).Execute();
                return new JsonResult(beautifulInfo);
            }

            if (endTime == null) // from query 
            {
                var beautifulInfo = (List<BySessionHour>)new Strategy(new SessionReturnFrom()).Execute(startTime);
                return new JsonResult(beautifulInfo);
            }

            if (startTime == null) // till query
            {
                var beautifulInfo = (List<BySessionHour>)new Strategy(new SessionReturnTill()).Execute(endTime);
                return new JsonResult(beautifulInfo);
            }

            if (startTime != null && endTime != null) // range query
            {
                var beautifulInfo = (List<BySessionHour>)new Strategy(new SessionReturnRange()).Execute(startTime, endTime);
                return new JsonResult(beautifulInfo);
            }

            return new JsonResult(new JsonObject()); // empty result
        }
    }
}