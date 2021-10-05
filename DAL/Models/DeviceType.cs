using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class DeviceType
    {
        public DeviceType()
        {
            RegistrationCountByDevicesAndMonths = new HashSet<RegistrationCountByDevicesAndMonth>();
        }

        public byte DeviceId { get; set; }
        public string DeviceName { get; set; }

        public virtual ICollection<RegistrationCountByDevicesAndMonth> RegistrationCountByDevicesAndMonths { get; set; }
    }
}
