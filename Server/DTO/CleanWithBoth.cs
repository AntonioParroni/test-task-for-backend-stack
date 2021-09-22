using System.Collections.Generic;

namespace Server.DTO
{
    public class CleanWithBoth
    {
        public int? year { get; set; }
        public byte? month { get; set; }
        public int? registeredUsers { get; set; }
        public List<Provision> registeredDevices { get; set; }
    }
}