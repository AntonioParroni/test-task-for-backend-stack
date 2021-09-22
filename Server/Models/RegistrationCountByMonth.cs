﻿#nullable disable

namespace Server.Models
{
    public partial class RegistrationCountByMonth
    {
        public int RegistrationCountByMonthId { get; set; }
        public short? Year { get; set; }
        public byte? Month { get; set; }
        public int? NumberOfUsers { get; set; }

        public virtual Month MonthNavigation { get; set; }
    }
}
