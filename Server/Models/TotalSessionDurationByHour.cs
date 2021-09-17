using System;
using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
    public partial class TotalSessionDurationByHour
    {
        public DateTime? Date { get; set; }
        public byte? Hour { get; set; }
        public int? TotalSessionDurationForHourInMins { get; set; }
        public int? TotalSessionDuration { get; set; }
    }
}
