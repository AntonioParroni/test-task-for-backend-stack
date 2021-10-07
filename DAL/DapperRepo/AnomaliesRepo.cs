using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DTO;
using Microsoft.Data.SqlClient;

#pragma warning disable 8604
#pragma warning disable 8629
#pragma warning disable 8602

namespace DAL.DapperRepo
{
    public interface IAnomaliesRepository
    {
        public List<CleanConcurrentLogins> GetAllAnomalies();
    }

    public class AnomaliesRepository : IAnomaliesRepository
    {
        string? connectionString;

        public AnomaliesRepository(string conn)
        {
            connectionString = conn;
        }

        public List<CleanConcurrentLogins> GetAllAnomalies()
        {
            List<CleanConcurrentLogins> returnInfo = new List<CleanConcurrentLogins>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql =
                    $"SELECT CR.UserName, CR.DeviceName AS device, CR.LoginTS AS loginTime, UCBD.Country AS country, UCBD.LoginTS AS LoginTime FROM ConcurrentUniqueSessionsWithMultipleDevices CR" +
                    " LEFT JOIN dbo.UniqueCountriesByDay UCBD on CR.UserName = UCBD.UserName AND UCBD.LoginTS = CR.LoginTS";

                var item = db.Query<CleanConcurrentLogins, CleanCountryLogin, CleanConcurrentLogins>(sql, (p, c) =>
                    {
                        p.unexpectedLogin = c;
                        return p;
                    }, splitOn: "Country")
                    .ToList();
                returnInfo = item;
            }

            return returnInfo;
        }
    }
}