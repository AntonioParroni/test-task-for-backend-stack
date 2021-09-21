using System;

namespace Server.ViewModel
{
    public class CleanConcurrentLogins
    {
        public string? userName { get; set; }
        public string? device { get; set; }
        public DateTime loginTime { get; set; }
        public CleanCountryLogin? unexpectedLogin { get; set; }
    }
}