using System;

#nullable disable

namespace DAL.Models
{
    public partial class ConcurrentSessionsEveryHour
    {
        public int ConcurrentSessionsEveryHourId { get; set; }
        public DateTime? Hour { get; set; }
        public int? NumberOfUsers { get; set; }
    }
}
