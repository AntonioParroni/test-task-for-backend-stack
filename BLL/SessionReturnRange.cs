using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.Models;
using DTO;
using Infrastructure;

namespace BLL
{
    public class SessionReturnRange : IStrategy
    {
        public object DoLogic(params object[] data)
        {
            string fromTimeString = data[0] as string;
            string tillTimeString = data[1] as string;

            DateTime fromTime;
            DateTime tillTime;
            try
            {
                fromTime = fromTimeString.ParseRequestTime();
                tillTime = tillTimeString.ParseRequestTime();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            
            DateTime tillDate = new DateTime(tillTime.Year, tillTime.Month, tillTime.Day);
            DateTime fromDate = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day);

            var crudeReturnInfo = new GenericRepository<TotalSessionDurationByHour>(new ApplicationContext()).Get();
            
            int fromHour = fromTime.Hour;
            int tillHour = tillTime.Hour;

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

                DateTime oldTime = new DateTime(year, month, day, (int)info.Hour, 0, 0); // this was the only way..

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