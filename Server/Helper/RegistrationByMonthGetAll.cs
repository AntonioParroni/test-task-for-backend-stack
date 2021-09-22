using System;
using System.Collections;
using System.Collections.Generic;
using Server.DTO;
using Server.Models;

namespace Server.Helper
{
    public class RegistrationByMonthGetAll : IStrategy
    {
        public object DoAlgorithm(params object[] data)
        {
            var okay = data[0] as List<RegistrationCountByMonth>;
            List<CleanByMonth> infoListToReturn = new List<CleanByMonth>();
            foreach (var crudeInfo in okay)
            {
                if (crudeInfo.Month == DateTime.Today.Month)
                {
                    CleanByMonth item = new CleanByMonth
                    {
                        year = crudeInfo.Year,
                        month = crudeInfo.Month,
                        registeredUsers = crudeInfo.NumberOfUsers
                    };
                    infoListToReturn.Add(item);
                }
            }
            return infoListToReturn;
        }
    }
}
