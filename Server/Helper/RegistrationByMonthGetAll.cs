using System;
using System.Collections;
using System.Collections.Generic;
using Server.DAL;
using Server.DTO;
using Server.Models;

namespace Server.Helper
{
    public class RegistrationByMonthGetAll : IStrategy
    {
        public object DoLogic(params object[] data)
        {
            var crudeInfoByMonth = new GenericRepository<RegistrationCountByMonth>(new ApplicationContext()).Get();
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

            return infoListToReturn;
        }
    }
}