using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Server.DAL;
using Server.DTO;
using Server.Models;

namespace Server.Logic.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private IRepository<RegistrationCountByMonth> _repoWithDates { get; set; }
        private  IRepository<RegistrationCountByDevicesAndMonth> _repoWithDataAndDevices { get; set; }
        private  IRepository<DeviceType> _deviceTypes { get; set; }


        public RegistrationService(IRepository<RegistrationCountByMonth> dates, 
            IRepository<RegistrationCountByDevicesAndMonth> devices,
            IRepository<DeviceType> deviceTypes)
        {
            _repoWithDates = dates;
            _repoWithDataAndDevices = devices;
            _deviceTypes = deviceTypes;
        }
        
        public JsonResult GetAll()
        {
            var crudeInfoByMonth = _repoWithDates.Get();
            List<CleanByMonth> infoListToReturn = new List<CleanByMonth>();
            foreach (var crudeInfo in crudeInfoByMonth)
            {
                if (crudeInfo.Month == DateTime.Today.Month)
                {
                    CleanByMonth item = new CleanByMonth
                    {
                        year = crudeInfo.Year, month = crudeInfo.Month, registeredUsers = crudeInfo.NumberOfUsers
                    };
                    infoListToReturn.Add(item);
                }
            }
            return new JsonResult(infoListToReturn);
        }

        public JsonResult GetById(int id)
        {
            int year = MySimpleMath.TakeNDigits(id, 4);
            int month = int.Parse((id % 100).ToString().PadLeft(2, '0'));
            var registrationByDeviceAndMonth = _repoWithDataAndDevices.Get();
            
            CleanWithBoth returnInfo = new CleanWithBoth();
            returnInfo.year = year;
            returnInfo.month = (byte)month;
            returnInfo.registeredUsers = 0;
            List<Provision> specificData = new List<Provision>();

            var devices = _deviceTypes.Get();
            foreach (var dataSet in registrationByDeviceAndMonth.Where(x => x.Year == year && x.Month == month))
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
            return new JsonResult(returnInfo);
        }
    }
}