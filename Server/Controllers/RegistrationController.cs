using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Models;
using Server.ViewModel;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/registration/bymonth")]
    public class RegistrationController : ControllerBase
    {
            public RegistrationController()
            {
                
            }
            
            [HttpGet]
            public ActionResult GetAll()
            {
                using (ApplicationContext context = new ApplicationContext())
                {
                    var crudeInfoByMonth = context.RegistrationCountByMonths.ToList();
                    var crudeInfoByDeviceAndMonth = context.RegistrationCountByDevicesAndMonths.ToList();

                    if (crudeInfoByMonth.Count != 0 && crudeInfoByDeviceAndMonth.Count != 0)
                    {
                        List<CleanByMonth> lst = new List<CleanByMonth>();
                        foreach (var crudeInfo in crudeInfoByMonth)
                        {
                            CleanByMonth item = new CleanByMonth();
                            item.Year = crudeInfo.Year;
                            item.Month = crudeInfo.Month;
                            item.NumberOfUsers = crudeInfo.NumberOfUsers;
                            lst.Add(item);
                        }

                        // List<Provision> prov = new List<Provision>();
                        // foreach (var info in lst)
                        // {
                        //     Provision ita = new Provision();
                        //     var smth = context.RegistrationCountByDevicesAndMonths.Where(x =>
                        //         x.Year == info.Year && x.Month == info.Month);
                        //     
                        // }
                        
                        return new JsonResult(lst);
                    }
                }
                // { year: 2021, month: 7, registeredUsers: 32, registeredDevices: [{ type: “laptop”, value: “15”}, { type: “mobile phone”,
                //     value: “8”,}, { type: “tablet”, value: “9”},] }
                return new JsonResult(null);;
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
                
                string str = "Year: " + year + " Month: " + month;
                
                
                return new JsonResult(str);
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