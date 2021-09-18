using System;
using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
    public partial class ConcurrentUniqueSessionsWithMultipleDevice
    {
        public int ConcurrentUniqueSessionsWithMultipleDevicesId { get; set; }
        public string UserName { get; set; }
        public string DeviceName { get; set; }
        public DateTime? LoginTs { get; set; }
    }
}
