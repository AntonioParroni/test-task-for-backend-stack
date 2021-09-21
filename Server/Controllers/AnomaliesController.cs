using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Models;
using Server.ViewModel;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/users/anomalies")]
    public class AnomaliesController : ControllerBase
    {
        private readonly ILogger<AnomaliesController> _logger;
            public AnomaliesController(ILogger<AnomaliesController> logger)
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
                    var crudeAnomalyConcurrentyLogins = context.ConcurrentUniqueSessionsWithMultipleDevices.ToList();
                    var crudeAnomalyCountriesLogins = context.UniqueCountriesByDays.ToList();

                    List<CleanConcurrentLogins> returnInfo = new List<CleanConcurrentLogins>();

                    foreach (var concurrentLoginElement in crudeAnomalyConcurrentyLogins)
                    {
                        CleanConcurrentLogins returnElement = new CleanConcurrentLogins();
                        returnElement.userName = concurrentLoginElement.UserName;
                        returnElement.device = concurrentLoginElement.DeviceName;
                        returnElement.loginTime = (DateTime)concurrentLoginElement.LoginTs;
                        foreach (var countriesLoginElement in crudeAnomalyCountriesLogins)
                        {
                            if (concurrentLoginElement.UserName == countriesLoginElement.UserName
                            && concurrentLoginElement.LoginTs == countriesLoginElement.LoginTs)
                            {
                                CleanCountryLogin unexpectedLogin = new CleanCountryLogin();
                                unexpectedLogin.country = countriesLoginElement.Country;
                                unexpectedLogin.loginTime = (DateTime)countriesLoginElement.LoginTs;
                                returnElement.unexpectedLogin = unexpectedLogin;
                            }
                        }
                        returnInfo.Add(returnElement);
                    }

                    if (returnInfo.Count == 0)
                    {
                        var emptyResult = new JObject();
                        return new JsonResult(emptyResult);
                    }
                    return new JsonResult(returnInfo);
                }
            }
            
            // POST action

            // PUT action

            // DELETE action
    }
}