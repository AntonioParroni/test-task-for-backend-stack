using System;
using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
    public partial class RegistrationCountByMonth
    {
        public short? Year { get; set; }
        public byte? Month { get; set; }
        public int? NumberOfUsers { get; set; }

        public virtual Month MonthNavigation { get; set; }
    }
}
