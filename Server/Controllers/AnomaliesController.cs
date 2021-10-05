using System.Collections.Generic;
using BLL;
using BLL.DapperRepo;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/users/anomalies")]
    public class AnomaliesController : ControllerBase
    {
        private readonly ILogger<AnomaliesController> _logger;
        private IAnomaliesRepository _repo;

        public AnomaliesController(ILogger<AnomaliesController> logger, IAnomaliesRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            _logger.LogInformation("Processing request: {0}", Request.Path + HttpContext.Request.QueryString);
            var beautifulInfo = _repo.GetAllAnomalies();
            return new JsonResult(beautifulInfo);
        }
    }
}