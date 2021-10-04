using System.Collections.Generic;
using System.Text.Json.Nodes;
using BLL;
using BLL.DapperRepo;
using DTO;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

#nullable enable

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/sessions/byhour")]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;
        private ISessionsRepository _repo;


        public SessionController(ILogger<SessionController> logger, ISessionsRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        public ActionResult Get([FromQuery] string? startTime, [FromQuery] string? endTime)
        {
            if (endTime == null && startTime == null) // no params case, return all
            {
                return new JsonResult(_repo.GetAllSessions());
            }

            if (endTime == null) // from query 
            {
                return new JsonResult(_repo.GetFromTime(startTime));
            }

            if (startTime == null) // till query
            {
                return new JsonResult(_repo.GetTillTime(endTime));            }

            if (startTime != null && endTime != null) // range query
            {
                return new JsonResult(_repo.GetRangeTime(startTime,endTime)); 
            }

            return new JsonResult(new JsonObject()); // empty result
        }
    }
}