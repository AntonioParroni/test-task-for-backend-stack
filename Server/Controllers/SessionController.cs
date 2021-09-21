using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                if (endTime == null && startTime == null) // no params case, return all
                {
                    using (ApplicationContext context = new ApplicationContext()) 
                    {
                        if (!context.Database.CanConnect())
                            return StatusCode(500);
                        var crudeReturnInfo = context.TotalSessionDurationByHours.ToList();
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
                            
                            DateTime oldTime = new DateTime(year, month, day, (int)info.Hour, 0, 0); // this is the only way..
                            
                            int? conccurentSessions = context.ConcurrentSessionsEveryHours
                                .Where(x => x.Hour == oldTime)
                                .Select(x => x.NumberOfUsers).FirstOrDefault();
                            value.conccurentSessions = conccurentSessions;
                            
                            beautifulInfo.Add(value);
                        }
                        return new JsonResult(beautifulInfo);
                    }
                }

                if (endTime == null) // from query 
                {
                    DateTime fromTime;
                    try
                    {
                        fromTime = new DateTime(int.Parse(startTime.Substring(0,4)),
                            int.Parse(startTime.Substring(5,2)),
                            int.Parse(startTime.Substring(8,2)), 
                            int.Parse(startTime.Substring(11,2)), 0, 0);
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
                        var crudeReturnInfo = context.TotalSessionDurationByHours.ToList();

                        DateTime onlyDate = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day);
                        int hour = fromTime.Hour;
                        
                        var parsedInfo = crudeReturnInfo.
                            Select(x => new 
                                {x.Date , x.Hour, x.TotalSessionDurationForHourInMins, x.TotalSessionDuration}).
                            Where(x => x.Date == onlyDate && x.Hour >= hour || x.Date > onlyDate).ToList(); // oh my gosh

                        if (parsedInfo.Count == 0)
                        {
                            var emptyResult = new JObject();
                            return new JsonResult(emptyResult);
                            // return StatusCode(404);
                        }
                           
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

                        if (beautifulInfo.Count == 0)
                        {
                            var emptyResult = new JObject();
                            return new JsonResult(emptyResult);
                        }
                        return new JsonResult(beautifulInfo);
                    }
                }

                if (startTime == null) // till query
                {
                    DateTime tillTime;
                    try
                    {
                        tillTime = new DateTime(int.Parse(endTime.Substring(0,4)),
                            int.Parse(endTime.Substring(5,2)),
                            int.Parse(endTime.Substring(8,2)), 
                            int.Parse(endTime.Substring(11,2)), 0, 0);
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
                        var crudeReturnInfo = context.TotalSessionDurationByHours.ToList();

                        DateTime onlyDate = new DateTime(tillTime.Year, tillTime.Month, tillTime.Day);
                        int hour = tillTime.Hour;
                        
                        var parsedInfo = crudeReturnInfo.
                            Select(x => new 
                                {x.Date , x.Hour, x.TotalSessionDurationForHourInMins, x.TotalSessionDuration}).
                            Where(x => x.Date == onlyDate && x.Hour <= hour || x.Date < onlyDate).ToList(); // oh my gosh

                        if (parsedInfo.Count == 0)
                        {
                            var emptyResult = new JObject();
                            return new JsonResult(emptyResult);
                            // return StatusCode(404);
                        }
                           
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

                        if (beautifulInfo.Count == 0)
                        {
                            var emptyResult = new JObject();
                            return new JsonResult(emptyResult);
                        }
                        return new JsonResult(beautifulInfo);
                    }
                }
                    
                if (startTime != null && endTime != null) // range query
                {
                    DateTime fromTime;
                    DateTime tillTime;
                    try
                    {
                        fromTime = new DateTime(int.Parse(startTime.Substring(0,4)),
                            int.Parse(startTime.Substring(5,2)),
                            int.Parse(startTime.Substring(8,2)), 
                            int.Parse(startTime.Substring(11,2)), 0, 0);
                        
                        tillTime = new DateTime(int.Parse(endTime.Substring(0,4)),
                            int.Parse(endTime.Substring(5,2)),
                            int.Parse(endTime.Substring(8,2)), 
                            int.Parse(endTime.Substring(11,2)), 0, 0);
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
                        var crudeReturnInfo = context.TotalSessionDurationByHours.ToList();

                        DateTime tillDate = new DateTime(tillTime.Year, tillTime.Month, tillTime.Day);
                        DateTime fromDate = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day);

                        int fromHour = fromTime.Hour;
                        int tillHour = tillTime.Hour;
                        
                        var parsedInfo = crudeReturnInfo.
                            Select(x => new 
                                {x.Date , x.Hour, x.TotalSessionDurationForHourInMins, x.TotalSessionDuration}).
                            Where(x => (x.Date == fromDate && x.Hour >= fromHour || x.Date > fromDate) 
                                       && (x.Date == tillDate && x.Hour <= tillHour || x.Date < tillDate)).ToList();
                        
                        if (parsedInfo.Count == 0)
                        {
                            var emptyResult = new JObject();
                            return new JsonResult(emptyResult);
                            // return StatusCode(404);
                        }
                           
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

                        if (beautifulInfo.Count == 0)
                        {
                            var emptyResult = new JObject();
                            return new JsonResult(emptyResult);
                        }
                        return new JsonResult(beautifulInfo);
                    }
                }
                return null;
            }

            // POST action

            // PUT action

            // DELETE action
    }
}