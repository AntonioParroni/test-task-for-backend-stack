using System;
using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
    public partial class RegistrationCountByDevicesAndMonth
    {
        public short? Year { get; set; }
        public byte? Month { get; set; }
        public byte? DeviceType { get; set; }
        public int? NumberOfUsers { get; set; }

        public virtual DeviceType DeviceTypeNavigation { get; set; }
        public virtual Month MonthNavigation { get; set; }
    }
}
