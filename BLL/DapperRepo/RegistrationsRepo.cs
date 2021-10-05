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
            List<CleanByMonth> infoListToReturn = new();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                int currMonth = DateTime.Today.Month;
                var crudeData = db.Query<RegistrationCountByMonth>(
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

        public CleanWithBoth GetRegistrationByID(int id)
        {
            int year = MySimpleMath.TakeNDigits(id, 4);
            int month = int.Parse((id % 100).ToString().PadLeft(2, '0'));
            CleanWithBoth returnInfo = new();
            returnInfo.year = year;
            returnInfo.month = (byte)month;
            returnInfo.registeredUsers = 0;
            List<Provision> specificData = new List<Provision>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var dataSet = db.Query<RegistrationCountByDevicesAndMonth>(
                        $"SELECT * FROM RegistrationCountByDevicesAndMonth WHERE Year = {year} AND Month = {month}")
                    .ToList();
                foreach (var crudeInfo in dataSet)
                {
                    Provision info = new Provision();
                    info.type = db.Query<DeviceType>(
                            $"SELECT DeviceName FROM DeviceTypes WHERE DeviceID = {crudeInfo.DeviceType.Value}")
                        .FirstOrDefault()
                        .DeviceName;
                    info.value = crudeInfo.NumberOfUsers;
                    specificData.Add(info);
                    if (crudeInfo.NumberOfUsers != null)
                    {
                        returnInfo.registeredUsers += crudeInfo.NumberOfUsers.Value;
                    }
                }

                returnInfo.registeredDevices = specificData;
            }

            return returnInfo;
        }
    }
}