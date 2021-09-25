using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.DAL;
using Server.DTO;
using Server.Logic;
using Server.Logic.Registration;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/registration/bymonth")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        public IRegistrationService _service { get; set; }

        public RegistrationController(ILogger<RegistrationController> logger, IRegistrationService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var returnInfo = _service.GetAll();
            if (returnInfo != null) return BadRequest(404);
            return returnInfo;
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            if (!ValidIdParser.Check(id))
                return StatusCode(404);
            var returnInfo = _service.GetById(id);
            if (returnInfo != null) return BadRequest(404);
            return returnInfo;
        }
    }
}