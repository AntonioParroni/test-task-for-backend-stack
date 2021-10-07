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

#pragma warning disable 8600
#pragma warning disable 8603
#pragma warning disable 8629
#pragma warning disable 8602

namespace DAL.DapperRepo
{
    public interface IRegistrationsRepository
    {
        public List<CleanByMonth> GetRegistrationByCurrentMonth();
        public CleanWithBoth GetRegistrationByID(int id);
    }

    public class RegistrationsRepository : IRegistrationsRepository
    {
        string? connectionString;

        public RegistrationsRepository(string? conn)
        {
            connectionString = conn;
        }

        public List<CleanByMonth> GetRegistrationByCurrentMonth()
        {
            List<CleanByMonth> returnInfo = new List<CleanByMonth>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                int currMonth = DateTime.Today.Month;
                var sql =
                    $"SELECT Year, Month, NumberOfUsers AS registeredUsers FROM RegistrationCountByMonth WHERE Month = {currMonth}";

                var item = db.Query<CleanByMonth>(sql).ToList();
                returnInfo = item;
            }

            return returnInfo;
        }

        public CleanWithBoth GetRegistrationByID(int id)
        {
            int year = MySimpleMath.TakeNDigits(id, 4);
            int month = int.Parse((id % 100).ToString().PadLeft(2, '0'));
            CleanWithBoth returnInfo = new();
            List<Provision> specificData = new List<Provision>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql =
                    $"SELECT RCDM.Year, RCDM.Month, SUM(NumberOfUsers) OVER() AS registeredUsers, RCDM.NumberOfUsers as value, DT.DeviceName AS type FROM RegistrationCountByDevicesAndMonth RCDM " +
                    " INNER JOIN DeviceTypes DT on DT.DeviceID = RCDM.DeviceType " +
                    $" WHERE Year = {year} AND Month = {month} " +
                    " group by RCDM.Month, RCDM.Year, RCDM.NumberOfUsers, DT.DeviceName";
                var item = db.Query<CleanWithBoth, Provision, CleanWithBoth>(sql, (p, c) =>
                    {
                        specificData.Add(c);
                        p.registeredDevices = specificData;
                        return p;
                    }, splitOn: "value")
                    .FirstOrDefault();
                returnInfo = item;
            }

            return returnInfo;
        }
    }
}