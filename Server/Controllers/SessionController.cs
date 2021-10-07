using System.Text.Json.Nodes;
using DAL.DapperRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
#pragma warning disable 8629
#pragma warning disable 8604

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
            _logger.LogInformation("Processing request: {0}", Request.Path + HttpContext.Request.QueryString);
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