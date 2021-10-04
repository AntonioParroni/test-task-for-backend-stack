using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.Models;
using Dapper;
using DTO;
using Microsoft.Data.SqlClient;
#pragma warning disable 8629

namespace BLL.DapperRepo
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
                var crudeAnomalyConcurrentyLogins =
                    db.Query<ConcurrentUniqueSessionsWithMultipleDevice>(
                            $"SELECT * FROM ConcurrentUniqueSessionsWithMultipleDevices")
                        .ToList();

                var crudeAnomalyCountriesLogins =
                    db.Query<UniqueCountriesByDay>($"SELECT * FROM UniqueCountriesByDay").ToList();

                foreach (var concurrentLoginElement in crudeAnomalyConcurrentyLogins)
                {
                    CleanConcurrentLogins returnElement = new CleanConcurrentLogins();
                    returnElement.userName = concurrentLoginElement.UserName;
                    returnElement.device = concurrentLoginElement.DeviceName;
                    returnElement.loginTime = (DateTime)concurrentLoginElement.LoginTs;
                    foreach (var countriesLoginElement in crudeAnomalyCountriesLogins)
                    {
                        if (concurrentLoginElement.UserName == countriesLoginElement.UserName &&
                            concurrentLoginElement.LoginTs == countriesLoginElement.LoginTs)
                        {
                            CleanCountryLogin unexpectedLogin = new CleanCountryLogin();
                            unexpectedLogin.country = countriesLoginElement.Country;
                            unexpectedLogin.loginTime = (DateTime)countriesLoginElement.LoginTs;
                            returnElement.unexpectedLogin = unexpectedLogin;
                        }
                    }

                    returnInfo.Add(returnElement);
                }
            }

            return returnInfo;
        }
    }
}