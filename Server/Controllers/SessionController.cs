using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Server.Models;
using Server.ViewModel;

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
                if (endTime == null && startTime == null)
                {
                    using (ApplicationContext context = new ApplicationContext()) // no params case, return all
                    {
                        if (!context.Database.CanConnect())
                            return StatusCode(500);
                        var crudeReturnInfo = context.TotalSessionDurationByHours.ToList();
                        var crudeInfoAboutConcurrentSessions = context.ConcurrentSessionsEveryHours.ToList(); // for further use
                        var parsedInfo = crudeReturnInfo.
                            Select(x => new 
                                { x.Date , x.Hour, x.TotalSessionDurationForHourInMins, x.TotalSessionDuration}).ToList();
                        if (parsedInfo.Count == 0)
                            return StatusCode(404);
                        List<BySessionHour> beautifulInfo = new List<BySessionHour>();
                        foreach (var info in parsedInfo) // parseIntoExpectedOutput
                        {
                            BySessionHour value = new BySessionHour();
                            string dateStr = info.Date.ToString().Remove(10);
                            int year = int.Parse(dateStr.Substring(dateStr.Length - 4));
                            int month = int.Parse(dateStr.Substring(0, 2));
                            int day = int.Parse(dateStr.Substring(3, 2));
                            dateStr = dateStr.Substring(dateStr.Length - 4) + "-" + dateStr.Substring(0,2) + "-" + dateStr.Substring(3,2);
                            value.date = dateStr;
                            value.hour = info.Hour;
                            value.qumulativeForHour = info.TotalSessionDurationForHourInMins;
                            value.totalTimeForHour = info.TotalSessionDuration;
                            
                            DateTime oldTime = new DateTime(year, month, day, (int)info.Hour, 0, 0); // this was the only way..
                            
                            int? conccurentSessions = context.ConcurrentSessionsEveryHours
                                .Where(x => x.Hour == oldTime)
                                .Select(x => x.NumberOfUsers).FirstOrDefault();
                            value.conccurentSessions = conccurentSessions;
                            
                            beautifulInfo.Add(value);
                        }
                        return new JsonResult(beautifulInfo);
                    }
                }

                if (endTime == null) // till query 
                {
                    // DateTime newTime = new DateTime(int.Parse(endTime.Substring(0,4)),
                    //     int.Parse(endTime.Substring(5,2)),
                    //     int.Parse(endTime.Substring(7,2)), 
                    //     int.Parse(endTime.Substring(9,2)), 0, 0);
                    
                    return new JsonResult("Till Query " + endTime);
                }

                if (startTime == null) // from query
                {
                    return new JsonResult("From Query" + endTime);
                }
                    
                if (startTime != null && endTime != null) // range query
                {
                    return new JsonResult("Range Query" + startTime + endTime);
                }
                    
                return StatusCode(404);
            }

            // POST action

            // PUT action

            // DELETE action
    }
}