using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Server.Models;
using Server.DTO;
using Server.Helper;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/sessions/byhour")]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;
        
            public SessionController(ILogger<SessionController> logger)
            {
                _logger = logger;
            }
            
            [HttpGet]
            public ActionResult Get([FromQuery] string? startTime, [FromQuery] string? endTime)
            {
                if (endTime == null && startTime == null) // no params case, return all
                {
                    using (ApplicationContext context = new ApplicationContext()) 
                    {
                        if (!context.Database.CanConnect())
                            return StatusCode(500);
                        StrategyContext newStrategy = new StrategyContext(new SessionReturnAll()); // logic selection
                        var beautifulInfo = (List<BySessionHour>)newStrategy.DoSomeLogic(context);
                        return new JsonResult(beautifulInfo);
                    }
                }

                if (endTime == null) // from query 
                {
                    DateTime fromTime;
                    try
                    {
                        fromTime = ParseRequestTime(startTime);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return StatusCode(400);
                    }
                    
                    using (ApplicationContext context = new ApplicationContext())
                    {
                        if (!context.Database.CanConnect())
                            return StatusCode(500);
                        StrategyContext newStrategy = new StrategyContext(new SessionReturnFrom()); // logic selection
                        var beautifulInfo = (List<BySessionHour>)newStrategy.DoSomeLogic(context, fromTime);
                        return new JsonResult(beautifulInfo);
                    }
                }

                if (startTime == null) // till query
                {
                    DateTime tillTime;
                    try
                    {
                        tillTime = ParseRequestTime(endTime);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return StatusCode(400);
                    }
                    
                    using (ApplicationContext context = new ApplicationContext())
                    {
                        if (!context.Database.CanConnect())
                            return StatusCode(500);
                        StrategyContext newStrategy = new StrategyContext(new SessionReturnTill()); // logic selection
                        var beautifulInfo = (List<BySessionHour>)newStrategy.DoSomeLogic(context, tillTime);
                        return new JsonResult(beautifulInfo);
                    }
                }
                    
                if (startTime != null && endTime != null) // range query
                {
                    DateTime fromTime;
                    DateTime tillTime;
                    try
                    {
                        fromTime = ParseRequestTime(startTime);
                        tillTime = ParseRequestTime(endTime);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return StatusCode(400);
                    }
                    
                    using (ApplicationContext context = new ApplicationContext())
                    {
                        if (!context.Database.CanConnect())
                            return StatusCode(500);
                        Tuple<DateTime?, DateTime?> requestParameters = new Tuple<DateTime?, DateTime?>(fromTime, tillTime);
                        StrategyContext newStrategy = new StrategyContext(new SessionReturnRange()); // // logic selection
                        var beautifulInfo = (List<BySessionHour>)newStrategy.DoSomeLogic(context, requestParameters);
                        return new JsonResult(beautifulInfo);
                    }
                }
                return null;
            }


            private DateTime ParseRequestTime(string str)
            {
                DateTime returnTime = new DateTime(int.Parse(str.Substring(0,4)),
                    int.Parse(str.Substring(5,2)),
                    int.Parse(str.Substring(8,2)), 
                    int.Parse(str.Substring(11,2)), 0, 0);
                return returnTime;
            }
            
            // POST action

            // PUT action

            // DELETE action
    }
}