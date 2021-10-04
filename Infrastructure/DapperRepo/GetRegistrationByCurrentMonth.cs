using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.Models;
using Dapper;
using DTO;
using Microsoft.Data.SqlClient;

namespace Infrastructure.DapperRepo
{
    public interface IRegsByCurrentMonth
    {
        public List<CleanByMonth> GetRegistrationByCurrentMonth();
    }
    public class RegsByCurrentMonthRepository : IRegsByCurrentMonth
    {
        string connectionString = null;
        public RegsByCurrentMonthRepository(string conn)
        {
            connectionString = conn;
        }
        public List<CleanByMonth> GetRegistrationByCurrentMonth()
        {
            List<CleanByMonth> infoListToReturn = new();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                int currMonth = DateTime.Today.Month;
                var crudeData =  db.Query<RegistrationCountByMonth>(
                    $"SELECT * FROM RegistrationCountByMonth WHERE Month = {currMonth}")
                    .ToList();
                foreach (var crudeInfo in crudeData)
                {
                    CleanByMonth item = new()
                        {
                            year = crudeInfo.Year, month = crudeInfo.Month, registeredUsers = crudeInfo.NumberOfUsers
                        };
                        infoListToReturn.Add(item);
                }
            }
            return infoListToReturn;
        }
    }
}