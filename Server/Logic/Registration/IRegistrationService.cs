using Microsoft.AspNetCore.Mvc;

namespace Server.Logic.Registration
{
    public interface IRegistrationService
    {
        public JsonResult GetAll();
        public JsonResult GetById(int id);
    }
}