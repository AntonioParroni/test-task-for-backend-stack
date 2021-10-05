using System;

#nullable disable

namespace DAL.Models
{
    public partial class UniqueCountriesByDay
    {
        public int UniqueCountriesByDayId { get; set; }
        public string UserName { get; set; }
        public string Country { get; set; }
        public DateTime? LoginTs { get; set; }
    }
}
