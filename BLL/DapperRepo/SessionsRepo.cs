using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BLL.Utils;
using DAL.Models;
using Dapper;
using DTO;
using Microsoft.Data.SqlClient;
#pragma warning disable 8629
#pragma warning disable 8602

namespace BLL.DapperRepo
{
    public interface ISessionsRepository
    {
        public List<BySessionHour> GetAllSessions();
        public List<BySessionHour> GetFromTime(string fromTime);
        public List<BySessionHour> GetTillTime(string tillTime);
        public List<BySessionHour> GetRangeTime(string fromTime, string tillTime);
    }

    public class SessionsRepository : ISessionsRepository
    {
        string? connectionString;

        public SessionsRepository(string? conn)
        {
            connectionString = conn;
        }

        public List<BySessionHour> GetAllSessions()
        {
            List<BySessionHour> beautifulReturnInfo = new List<BySessionHour>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sql =
                    "SELECT TSDH.Date, TSDH.Hour, TSDH.TotalSessionDurationForHourInMins AS totalTimeForHour, TSDH.TotalSessionDuration AS qumulativeForHour, " +
                    " CSEH.NumberOfUsers AS conccurentSessions FROM TotalSessionDurationByHour TSDH " +
                    " LEFT JOIN ConcurrentSessionsEveryHour CSEH ON dateadd(hour, TSDH.Hour, cast(TSDH.Date as datetime)) = CSEH.Hour";

                beautifulReturnInfo = db.Query<BySessionHour>(sql)
                    .ToList();
                
                // foreach (var info in crudeData)
                // {
                //     BySessionHour value = new BySessionHour();
                //     string dateStr = info.Date.ToString().Remove(10);
                //     int year = int.Parse(dateStr.Substring(dateStr.Length - 4));
                //     int month = int.Parse(dateStr.Substring(0, 2));
                //     int day = int.Parse(dateStr.Substring(3, 2));
                //     dateStr = dateStr.Substring(dateStr.Length - 4) + "-" + dateStr.Substring(0, 2) + "-" +
                //               dateStr.Substring(3, 2);
                //     value.date = dateStr;
                //     value.hour = info.Hour;
                //     value.qumulativeForHour = info.TotalSessionDurationForHourInMins;
                //     value.totalTimeForHour = info.TotalSessionDuration;
                //     DateTime oldTime = new DateTime(year, month, day, (int)info.Hour, 0, 0); // this is the only way..
                //     string sqlFormattedDate = oldTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                //     string query =
                //         $"SELECT NumberOfUsers FROM ConcurrentSessionsEveryHour WHERE Hour = '{sqlFormattedDate}'";
                //     int? conccurentSessions = int.Parse(db.Query<int>(query).FirstOrDefault().ToString());
                //     value.conccurentSessions = conccurentSessions;
                //     beautifulReturnInfo.Add(value);
                // }
            }

            if (beautifulReturnInfo.Count == 0)
                return beautifulReturnInfo;
            foreach (var session in beautifulReturnInfo)
            {
                if (session.conccurentSessions == null)
                {
                    session.conccurentSessions = 0;
                }
                DateTime sessionDate = DateTime.Parse(session.date);
                session.date = sessionDate.ToString("yyyy-MM-dd");
            }
            return beautifulReturnInfo;
        }

        public List<BySessionHour> GetFromTime(string fromTime)
        {
            List<BySessionHour> beautifulReturnInfo = new List<BySessionHour>();
            DateTime fromTimeDate;
            try
            {
                fromTimeDate = fromTime.ParseRequestTime();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return beautifulReturnInfo;
            }

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                DateTime onlyDate = new DateTime(fromTimeDate.Year, fromTimeDate.Month, fromTimeDate.Day);
                int hour = fromTimeDate.Hour;
                string sqlFormattedDate = onlyDate.ToString("yyyy-MM-dd");
                string queryStr =
                    $"SELECT Date, Hour, TotalSessionDurationForHourInMins, TotalSessionDuration FROM TotalSessionDurationByHour" +
                    $" WHERE Date = '{sqlFormattedDate}' AND Hour >= {hour} OR Date > '{sqlFormattedDate}'";
                var crudeData = db.Query<TotalSessionDurationByHour>(queryStr).ToList();
                if (crudeData.Count == 0) return beautifulReturnInfo;

                foreach (var info in crudeData)
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
                    DateTime oldTime = new DateTime(year, month, day, (int)info.Hour, 0, 0);
                    string sqlFormattedDate2 = oldTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string query =
                        $"SELECT NumberOfUsers FROM ConcurrentSessionsEveryHour WHERE Hour = '{sqlFormattedDate2}'";
                    int? conccurentSessions = int.Parse(db.Query<int>(query).FirstOrDefault().ToString());
                    value.conccurentSessions = conccurentSessions;
                    beautifulReturnInfo.Add(value);
                }
            }

            return beautifulReturnInfo;
        }

        public List<BySessionHour> GetTillTime(string tillTime)
        {
            List<BySessionHour> beautifulReturnInfo = new List<BySessionHour>();
            DateTime tillTimeDate;
            try
            {
                tillTimeDate = tillTime.ParseRequestTime();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return beautifulReturnInfo;
            }

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                DateTime onlyDate = new DateTime(tillTimeDate.Year, tillTimeDate.Month, tillTimeDate.Day);
                int hour = tillTimeDate.Hour;
                string sqlFormattedDate = onlyDate.ToString("yyyy-MM-dd");
                string queryStr =
                    $"SELECT Date, Hour, TotalSessionDurationForHourInMins, TotalSessionDuration FROM TotalSessionDurationByHour" +
                    $" WHERE Date = '{sqlFormattedDate}' AND Hour <= {hour} OR Date < '{sqlFormattedDate}'";
                var crudeData = db.Query<TotalSessionDurationByHour>(queryStr).ToList();
                if (crudeData.Count == 0) return beautifulReturnInfo;

                foreach (var info in crudeData)
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
                    DateTime oldTime = new DateTime(year, month, day, (int)info.Hour, 0, 0);
                    string sqlFormattedDate2 = oldTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string query =
                        $"SELECT NumberOfUsers FROM ConcurrentSessionsEveryHour WHERE Hour = '{sqlFormattedDate2}'";
                    int? conccurentSessions = int.Parse(db.Query<int>(query).FirstOrDefault().ToString());
                    value.conccurentSessions = conccurentSessions;
                    beautifulReturnInfo.Add(value);
                }
            }

            return beautifulReturnInfo;
        }

        public List<BySessionHour> GetRangeTime(string fromTime, string tillTime)
        {
            List<BySessionHour> beautifulReturnInfo = new List<BySessionHour>();
            DateTime fromTimeDate;
            DateTime tillTimeDate;

            try
            {
                fromTimeDate = fromTime.ParseRequestTime();
                tillTimeDate = tillTime.ParseRequestTime();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return beautifulReturnInfo;
            }

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                DateTime tillDate = new DateTime(tillTimeDate.Year, tillTimeDate.Month, tillTimeDate.Day);
                DateTime fromDate = new DateTime(fromTimeDate.Year, fromTimeDate.Month, fromTimeDate.Day);

                int fromHour = fromTimeDate.Hour;
                int tillHour = tillTimeDate.Hour;

                string sqlFormattedFromDate = fromDate.ToString("yyyy-MM-dd");
                string sqlFormattedTillDate = tillDate.ToString("yyyy-MM-dd");

                string queryStr =
                    $"SELECT Date, Hour, TotalSessionDurationForHourInMins, TotalSessionDuration FROM TotalSessionDurationByHour" +
                    $" WHERE (Date = '{sqlFormattedFromDate}' AND Hour >= {fromHour} OR Date > '{sqlFormattedFromDate}')" +
                    $" AND (Date = '{sqlFormattedTillDate}' AND Hour <= {tillHour} OR Date < '{tillDate}')";

                var crudeData = db.Query<TotalSessionDurationByHour>(queryStr).ToList();

                if (crudeData.Count == 0) return beautifulReturnInfo;

                foreach (var info in crudeData)
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
                    DateTime oldTime = new DateTime(year, month, day, (int)info.Hour, 0, 0);
                    string sqlFormattedDate2 = oldTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string query =
                        $"SELECT NumberOfUsers FROM ConcurrentSessionsEveryHour WHERE Hour = '{sqlFormattedDate2}'";
                    int? conccurentSessions = int.Parse(db.Query<int>(query).FirstOrDefault().ToString());
                    value.conccurentSessions = conccurentSessions;
                    beautifulReturnInfo.Add(value);
                }
            }

            return beautifulReturnInfo;
        }
    }
}