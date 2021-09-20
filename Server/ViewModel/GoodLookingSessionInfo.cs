using System;

namespace Server.ViewModel
{
    public class GoodLookingSessionInfo
    {
        public DateTime? date { get; set; }
        public byte? hour { get; set; }
        public int? concurrentSessions { get; set; }
        public int? totalTimeForHour { get; set; }
        public int? qumulativeForHour { get; set; }
    }
}