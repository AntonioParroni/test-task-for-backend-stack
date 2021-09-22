using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Server.DAL;
using Server.DTO;
using Server.Models;

namespace Server.Helper
{
    public class RegistrationByMonthGetByID : IStrategy
    {
        public object DoLogic(params object[] data)
        {
            int year = MySimpleMath.TakeNDigits((int)data[0], 4);
            int month = int.Parse(((int)data[0] % 100).ToString().PadLeft(2, '0'));
            var registrationByDeviceAndMonth =
                new GenericRepository<RegistrationCountByDevicesAndMonth>(new ApplicationContext());

            CleanWithBoth returnInfo = new CleanWithBoth();
            returnInfo.year = year;
            returnInfo.month = (byte)month;
            returnInfo.registeredUsers = 0;
            List<Provision> specificData = new List<Provision>();

            var devices = new GenericRepository<DeviceType>(new ApplicationContext()).Get();
            foreach (var dataSet in registrationByDeviceAndMonth.Get(x => x.Year == year && x.Month == month))
            {
                Provision info = new Provision();
                info.type = devices.First(x => x.DeviceId == dataSet.DeviceType.Value).DeviceName;
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
    }
}