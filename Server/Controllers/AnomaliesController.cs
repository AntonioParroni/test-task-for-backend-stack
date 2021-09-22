using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Models;
using Server.DTO;
using Server.Helper;

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
            using (ApplicationContext context = new ApplicationContext())
            {
                if (!context.Database.CanConnect()) return StatusCode(500);
                StrategyContext newStrategy = new StrategyContext(new AnomaliesReturnAll()); // logic selection
                var beautifulInfo = (List<CleanConcurrentLogins>)newStrategy.DoSomeLogic(context);
                return new JsonResult(beautifulInfo);
            }
        }

        // POST action

        // PUT action

        // DELETE action
    }
}