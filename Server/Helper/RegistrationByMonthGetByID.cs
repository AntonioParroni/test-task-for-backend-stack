using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Models;

namespace Server.Helper
{
    public class RegistrationByMonthGetByID : IStrategy
    {
        public object DoLogic(params object[] data)
        {
            var dbContext = data[0] as ApplicationContext;
            Tuple<int, int> requestParameters = data[1] as Tuple<int, int>;
            var crudeInfoByDeviceAndMonth = dbContext.RegistrationCountByDevicesAndMonths.ToList();
            if (crudeInfoByDeviceAndMonth.Count != 0)
            {
                CleanWithBoth returnInfo = new CleanWithBoth();
                returnInfo.year = (short)requestParameters.Item1;
                returnInfo.month = (byte)requestParameters.Item2;
                returnInfo.registeredUsers = 0;

                List<Provision> specificData = new List<Provision>();
                foreach (var dataSet in crudeInfoByDeviceAndMonth.Where(x =>
                    x.Year == requestParameters.Item1 && x.Month == requestParameters.Item2))
                {
                    Provision info = new Provision();
                    info.type = dbContext.DeviceTypes.First(c => c.DeviceId == dataSet.DeviceType.Value).DeviceName;
                    info.value = dataSet.NumberOfUsers;
                    specificData.Add(info);
                    if (dataSet.NumberOfUsers != null)
                    {
                        returnInfo.registeredUsers += dataSet.NumberOfUsers.Value;
                    }
                }

                returnInfo.registeredDevices = specificData;
                return returnInfo;
            }

            return null;
        }
    }
}