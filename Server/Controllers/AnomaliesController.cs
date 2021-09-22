using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.DTO;
using Server.Logic;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/users/anomalies")]
    public class AnomaliesController : ControllerBase
    {
        private readonly ILogger<AnomaliesController> _logger;

        public AnomaliesController(ILogger<AnomaliesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var beautifulInfo = (List<CleanConcurrentLogins>)new Strategy(new AnomaliesReturnAll()).Execute();
            return new JsonResult(beautifulInfo);
        }
    }
}