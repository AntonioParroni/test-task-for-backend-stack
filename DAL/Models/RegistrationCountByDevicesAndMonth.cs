﻿#nullable disable

namespace DAL.Models
{
    public partial class RegistrationCountByDevicesAndMonth
    {
        public int RegistrationCountByDevicesAndMonthId { get; set; }
        public short? Year { get; set; }
        public byte? Month { get; set; }
        public byte? DeviceType { get; set; }
        public int? NumberOfUsers { get; set; }

        public virtual DeviceType DeviceTypeNavigation { get; set; }
        public virtual Month MonthNavigation { get; set; }
    }
}