using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Models;
using Server.DTO;
using Server.Helper;

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
            using (ApplicationContext context = new ApplicationContext())
            {
                if (!context.Database.CanConnect()) return StatusCode(500);
                var crudeInfoByMonth = context.RegistrationCountByMonths.ToList();
                if (crudeInfoByMonth.Count != 0)
                {
                    StrategyContext
                        context1 = new StrategyContext(new RegistrationByMonthGetAll()); // setting the correct strategy
                    var infoListToReturn = (List<CleanByMonth>)context1.DoSomeLogic(crudeInfoByMonth);
                    if (infoListToReturn.Count == 0) return BadRequest(404);
                    return new JsonResult(infoListToReturn);
                }
            }

            return BadRequest(404);
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            if (Math.Floor(Math.Log10(id) + 1) > 6) // check for six digits
                return StatusCode(400);
            int year = MySimpleMath.TakeNDigits(id, 4);
            int month = int.Parse((id % 100).ToString().PadLeft(2, '0'));

            if (!Enumerable.Range(1, 12).Contains(month)) // check for a valid month
                return StatusCode(400);
            if (!Enumerable.Range(2020, 2021).Contains(year)) // check for a valid year
                return StatusCode(400);

            using (ApplicationContext context = new ApplicationContext())
            {
                if (!context.Database.CanConnect()) return StatusCode(500);

                var RequestParameters = new Tuple<int, int>(year, month);
                StrategyContext newStrategy = new StrategyContext(new RegistrationByMonthGetByID());
                var returnInfo = (CleanWithBoth)newStrategy.DoSomeLogic(context, RequestParameters);

                if (returnInfo.registeredUsers == 0) return BadRequest(404);
                return new JsonResult(returnInfo);
            }
        }

        // POST action

        // PUT action

        // DELETE action
    }
}