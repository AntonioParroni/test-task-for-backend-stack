using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Models;
using Server.ViewModel;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/registration/bymonth/[controller]")]
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

            // GET by Id action

            // POST action

            // PUT action

            // DELETE action
        
    }
}