using Server.Models;

namespace Server.ViewModel
{
    public class CleanByMonthAndDevice
    {
        public short? Year { get; set; }
        public byte? Month { get; set; }
        public string? DeviceType { get; set; }
        public int? NumberOfUsers { get; set; }
    }
}