using System;
using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
    public partial class UniqueCountriesByDay
    {
        public string UserName { get; set; }
        public string Country { get; set; }
        public DateTime? LoginTs { get; set; }
    }
}
