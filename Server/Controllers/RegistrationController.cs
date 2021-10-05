using BLL.DapperRepo;
using BLL.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/registration/bymonth")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private IRegistrationsRepository _repo;

        public RegistrationController(ILogger<RegistrationController> logger, IRegistrationsRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var returnInfo = _repo.GetRegistrationByCurrentMonth();
            if (returnInfo.Count == 0) return BadRequest(404);
            return new JsonResult(returnInfo);
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            if (!ValidIdParser.Check(id))
                return StatusCode(404);
            var returnInfo = _repo.GetRegistrationByID(id);
            if (returnInfo.registeredUsers == 0) return BadRequest(404);
            return new JsonResult(returnInfo);
        }
    }
}