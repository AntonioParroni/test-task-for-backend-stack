using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BLL.Utils;
using Dapper;
using DTO;
using Microsoft.Data.SqlClient;

#pragma warning disable 8604
#pragma warning disable 8629
#pragma warning disable 8602

namespace DAL.DapperRepo
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

                beautifulReturnInfo = db.Query<BySessionHour>(sql).ToList();
            }

            if (beautifulReturnInfo.Count == 0) return beautifulReturnInfo;
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

            DateTime onlyDate = new DateTime(fromTimeDate.Year, fromTimeDate.Month, fromTimeDate.Day);
            int hour = fromTimeDate.Hour;
            string sqlFormattedDate = onlyDate.ToString("yyyy-MM-dd");

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sql =
                    "SELECT TSDH.Date, TSDH.Hour, TSDH.TotalSessionDurationForHourInMins AS totalTimeForHour, TSDH.TotalSessionDuration AS qumulativeForHour, " +
                    " CSEH.NumberOfUsers AS conccurentSessions FROM TotalSessionDurationByHour TSDH " +
                    " LEFT JOIN ConcurrentSessionsEveryHour CSEH ON dateadd(hour, TSDH.Hour, cast(TSDH.Date as datetime)) = CSEH.Hour" +
                    $" WHERE TSDH.Date = '{sqlFormattedDate}' AND TSDH.Hour >= {hour} OR TSDH.Date > '{sqlFormattedDate}'";
                
                beautifulReturnInfo = db.Query<BySessionHour>(sql).ToList();
            }

            if (beautifulReturnInfo.Count == 0) return beautifulReturnInfo;
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

            DateTime onlyDate = new DateTime(tillTimeDate.Year, tillTimeDate.Month, tillTimeDate.Day);
            int hour = tillTimeDate.Hour;
            string sqlFormattedDate = onlyDate.ToString("yyyy-MM-dd");

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sql =
                    "SELECT TSDH.Date, TSDH.Hour, TSDH.TotalSessionDurationForHourInMins AS totalTimeForHour, TSDH.TotalSessionDuration AS qumulativeForHour, " +
                    " CSEH.NumberOfUsers AS conccurentSessions FROM TotalSessionDurationByHour TSDH " +
                    " LEFT JOIN ConcurrentSessionsEveryHour CSEH ON dateadd(hour, TSDH.Hour, cast(TSDH.Date as datetime)) = CSEH.Hour" +
                    $" WHERE TSDH.Date = '{sqlFormattedDate}' AND TSDH.Hour <= {hour} OR TSDH.Date < '{sqlFormattedDate}'";

                beautifulReturnInfo = db.Query<BySessionHour>(sql).ToList();
            }

            if (beautifulReturnInfo.Count == 0) return beautifulReturnInfo;
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

            DateTime tillDate = new DateTime(tillTimeDate.Year, tillTimeDate.Month, tillTimeDate.Day);
            DateTime fromDate = new DateTime(fromTimeDate.Year, fromTimeDate.Month, fromTimeDate.Day);

            int fromHour = fromTimeDate.Hour;
            int tillHour = tillTimeDate.Hour;

            string sqlFormattedFromDate = fromDate.ToString("yyyy-MM-dd");
            string sqlFormattedTillDate = tillDate.ToString("yyyy-MM-dd");

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sql =
                    "SELECT TSDH.Date, TSDH.Hour, TSDH.TotalSessionDurationForHourInMins AS totalTimeForHour, TSDH.TotalSessionDuration AS qumulativeForHour, " +
                    " CSEH.NumberOfUsers AS conccurentSessions FROM TotalSessionDurationByHour TSDH " +
                    " LEFT JOIN ConcurrentSessionsEveryHour CSEH ON dateadd(hour, TSDH.Hour, cast(TSDH.Date as datetime)) = CSEH.Hour" +
                    $" WHERE (TSDH.Date = '{sqlFormattedFromDate}' AND TSDH.Hour >= {fromHour} OR TSDH.Date > '{sqlFormattedFromDate}')" +
                    $" AND (TSDH.Date = '{sqlFormattedTillDate}' AND TSDH.Hour <= {tillHour} OR TSDH.Date < '{tillDate}')";

                beautifulReturnInfo = db.Query<BySessionHour>(sql).ToList();
            }

            if (beautifulReturnInfo.Count == 0) return beautifulReturnInfo;
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
    }
}