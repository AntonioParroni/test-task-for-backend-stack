using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Server.DTO;
using Server.Models;

namespace Server.Helper
{
    public class SessionReturnRange : IStrategy
    {
        public object DoLogic(params object[] data)
        {
            var dbContext = data[0] as ApplicationContext;
            Tuple<DateTime?, DateTime?> Time = data[1] as Tuple<DateTime?, DateTime?> ?? default;
            var crudeReturnInfo = dbContext.TotalSessionDurationByHours.ToList();

            DateTime tillDate = new DateTime(Time.Item2.Value.Year, Time.Item2.Value.Month, Time.Item2.Value.Day);
            DateTime fromDate = new DateTime(Time.Item1.Value.Year, Time.Item1.Value.Month, Time.Item1.Value.Day);

            int fromHour = Time.Item1.Value.Hour;
            int tillHour = Time.Item2.Value.Hour;

            var parsedInfo = crudeReturnInfo
                .Select(x => new { x.Date, x.Hour, x.TotalSessionDurationForHourInMins, x.TotalSessionDuration })
                .Where(x => (x.Date == fromDate && x.Hour >= fromHour || x.Date > fromDate) &&
                            (x.Date == tillDate && x.Hour <= tillHour || x.Date < tillDate))
                .ToList();

            List<BySessionHour> beautifulInfo = new List<BySessionHour>();
            if (parsedInfo.Count == 0)
            {
                return beautifulInfo;
            }

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

                DateTime oldTime = new DateTime(year, month, day, (int)info.Hour, 0, 0); // this was the only way..

                int? conccurentSessions = dbContext.ConcurrentSessionsEveryHours.Where(x => x.Hour == oldTime)
                    .Select(x => x.NumberOfUsers)
                    .FirstOrDefault();
                value.conccurentSessions = conccurentSessions;

                beautifulInfo.Add(value);
            }

            return beautifulInfo;
        }
    }
}