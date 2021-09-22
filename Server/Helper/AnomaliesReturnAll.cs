using System;
using System.Collections.Generic;
using System.Linq;
using Server.DTO;
using Server.Models;

namespace Server.Helper
{
    public class AnomaliesReturnAll : IStrategy
    {
        public object DoLogic(params object[] data)
        {
            var dbContext = data[0] as ApplicationContext;
            var crudeAnomalyConcurrentyLogins = dbContext.ConcurrentUniqueSessionsWithMultipleDevices.ToList();
            var crudeAnomalyCountriesLogins = dbContext.UniqueCountriesByDays.ToList();
            List<CleanConcurrentLogins> returnInfo = new List<CleanConcurrentLogins>();

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

            return returnInfo;
        }
    }
}