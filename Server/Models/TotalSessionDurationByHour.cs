using System;

#nullable disable

namespace Server.Models
{
    public partial class TotalSessionDurationByHour
    {
        public int TotalSessionDurationByHourId { get; set; }
        public DateTime? Date { get; set; }
        public byte? Hour { get; set; }
        public int? TotalSessionDurationForHourInMins { get; set; }
        public int? TotalSessionDuration { get; set; }
    }
}
