using System.Collections.Generic;
using BLL;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/registration/bymonth")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(ILogger<RegistrationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var returnInfo = (List<CleanByMonth>)new Strategy(new RegistrationByMonthGetAll()).Execute();
            if (returnInfo.Count == 0) return BadRequest(404);
            return new JsonResult(returnInfo);
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            if (!ValidIdParser.Check(id))
                return StatusCode(404);
            var returnInfo = (CleanWithBoth)new Strategy(new RegistrationByMonthGetByID()).Execute(id);
            if (returnInfo.registeredUsers == 0) return BadRequest(404);
            return new JsonResult(returnInfo);
        }
    }
}