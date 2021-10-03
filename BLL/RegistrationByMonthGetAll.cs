using System;
using System.Collections.Generic;
using DAL;
using DAL.Models;
using DTO;

namespace BLL
{
    public class RegistrationByMonthGetAll : IStrategy
    {
        public object DoLogic(params object[] data)
        {
            var crudeInfoByMonth = new GenericRepository<RegistrationCountByMonth>(new ApplicationContext()).Get();
            List<CleanByMonth> infoListToReturn = new();
            foreach (var crudeInfo in crudeInfoByMonth)
            {
                if (crudeInfo.Month == DateTime.Today.Month)
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