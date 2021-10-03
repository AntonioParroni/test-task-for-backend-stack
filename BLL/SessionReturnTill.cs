using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.Models;
using DTO;

namespace BLL
{
    public class SessionReturnTill : IStrategy
    {
        public object DoLogic(params object[] data)
        {
            string time = data[0] as string;
            DateTime tillTime;
            try
            {
                tillTime = time.ParseRequestTime();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            var crudeReturnInfo = new GenericRepository<TotalSessionDurationByHour>(new ApplicationContext()).Get();
            DateTime onlyDate = new DateTime(tillTime.Year, tillTime.Month, tillTime.Day);
            int hour = tillTime.Hour;

            var parsedInfo = crudeReturnInfo
                .Select(x => new { x.Date, x.Hour, x.TotalSessionDurationForHourInMins, x.TotalSessionDuration })
                .Where(x => x.Date == onlyDate && x.Hour <= hour || x.Date < onlyDate)
                .ToList(); // oh my gosh

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