using System;
using System.Collections.Generic;
using System.Linq;
using Server.DAL;
using Server.DTO;
using Server.Models;

namespace Server.Logic
{
    public class SessionReturnAll : IStrategy
    {
        public object DoLogic(params object[] data)
        {
            var crudeReturnInfo = new GenericRepository<TotalSessionDurationByHour>(new ApplicationContext()).Get();
            var parsedInfo = crudeReturnInfo.Select(x =>
                    new { x.Date, x.Hour, x.TotalSessionDurationForHourInMins, x.TotalSessionDuration })
                .ToList();
            List<BySessionHour> beautifulInfo = new List<BySessionHour>();
            if (parsedInfo.Count == 0) return beautifulInfo;

            var devices = new GenericRepository<ConcurrentSessionsEveryHour>(new ApplicationContext()).Get();

            foreach (var info in parsedInfo) // parseIntoExpectedOutput
            {
                BySessionHour value = new BySessionHour();
                string dateStr = info.Date.ToString().Remove(10);
                int year = int.Parse(dateStr.Substring(dateStr.Length - 4));
                int month = int.Parse(dateStr.Substring(0, 2));
                int day = int.Parse(dateStr.Substring(3, 2));
                dateStr = dateStr.Substring(dateStr.Length - 4) + "-" + dateStr.Substring(0, 2) + "-" +
                          dateStr.Substring(3, 2);
                value.date = dateStr;
                value.hour = info.Hour;
                value.qumulativeForHour = info.TotalSessionDurationForHourInMins;
                value.totalTimeForHour = info.TotalSessionDuration;
                DateTime oldTime = new DateTime(year, month, day, (int)info.Hour, 0, 0); // this is the only way..
                int? conccurentSessions = devices.Where(x => x.Hour == oldTime)
                    .Select(x => x.NumberOfUsers)
                    .FirstOrDefault();
                value.conccurentSessions = conccurentSessions;
                beautifulInfo.Add(value);
            }

            return beautifulInfo;
        }
    }
}