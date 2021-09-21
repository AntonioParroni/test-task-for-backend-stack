using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Attributes;
using Server.Models;
using Server.ViewModel;

namespace Server.Controllers
{
    [ApiKey]
    [ApiController]
    [Route("/api/registration/bymonth")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
            public RegistrationController(ILogger<RegistrationController> logger)
            {
                _logger = logger;
            }
            
            [HttpGet]
            public ActionResult GetAll()
            {
                using (ApplicationContext context = new ApplicationContext())
                {
                    if (!context.Database.CanConnect())
                        return StatusCode(500);
                    var crudeInfoByMonth = context.RegistrationCountByMonths.ToList();
                    if (crudeInfoByMonth.Count != 0)
                    {
                        List<CleanByMonth> infoListToReturn = new List<CleanByMonth>();
                        foreach (var crudeInfo in crudeInfoByMonth)
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
                        if (infoListToReturn.Count == 0)
                            return StatusCode(404);
                        return new JsonResult(infoListToReturn);
                    }
                }
                return StatusCode(404);
            }

            [HttpGet("{id}")]
            public ActionResult Get(int id)
            {
                if (Math.Floor(Math.Log10(id) + 1) > 6) // check for six digits
                    return StatusCode(400);
                int year = takeNDigits(id, 4);
                int month = int.Parse((id % 100).ToString().PadLeft(2, '0'));
                
                if (!Enumerable.Range(1,12).Contains(month)) // check for a valid month
                    return StatusCode(400);
                if (!Enumerable.Range(2020,2021).Contains(year)) // check for a valid year
                    return StatusCode(400);

                using (ApplicationContext context = new ApplicationContext())
                {
                    if (!context.Database.CanConnect())
                        return StatusCode(500);
                    var crudeInfoByDeviceAndMonth = context.RegistrationCountByDevicesAndMonths.ToList();
                    if (crudeInfoByDeviceAndMonth.Count != 0)
                    {
                        CleanWithBoth returnInfo = new CleanWithBoth();
                        returnInfo.year = (short)year;
                        returnInfo.month = (byte)month;
                        returnInfo.registeredUsers = 0;
                        
                        List<Provision> specificData = new List<Provision>();
                        foreach (var dataSet in crudeInfoByDeviceAndMonth.
                            Where(x => x.Year == year && x.Month == month))
                        {
                            Provision info = new Provision();
                            info.type = context.DeviceTypes.First(c => c.DeviceId == dataSet.DeviceType.Value).DeviceName;
                            info.value = dataSet.NumberOfUsers;
                            specificData.Add(info);
                            if (dataSet.NumberOfUsers != null)
                            {
                                returnInfo.registeredUsers += dataSet.NumberOfUsers.Value;
                            }
                        }
                        returnInfo.registeredDevices = specificData;
                        if (returnInfo.registeredUsers == 0)
                            return StatusCode(404);
                        return new JsonResult(returnInfo);
                    }
                }
                return StatusCode(404);
            }

            // POST action

            // PUT action

            // DELETE action
            
            private static int takeNDigits(int number, int N)
            {
                number = Math.Abs(number);
                if(number == 0)
                    return number;
                int numberOfDigits = (int)Math.Floor(Math.Log10(number) + 1);
                if (numberOfDigits >= N)
                    return (int)Math.Truncate((number / Math.Pow(10, numberOfDigits - N)));
                return number;
            }
        
    }
}