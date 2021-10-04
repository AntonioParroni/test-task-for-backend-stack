using System.Collections.Generic;
using BLL;
using DTO;
using Infrastructure;
using Infrastructure.DapperRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/registration/bymonth")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private IRegsByCurrentMonth _repo;  

        public RegistrationController(ILogger<RegistrationController> logger, IRegsByCurrentMonth repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            // var returnInfo = (List<CleanByMonth>)new Strategy(new RegistrationByMonthGetAll()).Execute();
            var returnInfo = _repo.GetRegistrationByCurrentMonth();
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