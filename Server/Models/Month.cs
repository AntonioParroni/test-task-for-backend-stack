using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
    public partial class Month
    {
        public Month()
        {
            RegistrationCountByDevicesAndMonths = new HashSet<RegistrationCountByDevicesAndMonth>();
            RegistrationCountByMonths = new HashSet<RegistrationCountByMonth>();
        }

        public byte MonthId { get; set; }
        public string MonthName { get; set; }

        public virtual ICollection<RegistrationCountByDevicesAndMonth> RegistrationCountByDevicesAndMonths { get; set; }
        public virtual ICollection<RegistrationCountByMonth> RegistrationCountByMonths { get; set; }
    }
}
