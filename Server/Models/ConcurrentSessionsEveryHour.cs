﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
    public partial class ConcurrentSessionsEveryHour
    {
        public int ConcurrentSessionsEveryHourId { get; set; }
        public DateTime? Hour { get; set; }
        public int? NumberOfUsers { get; set; }
    }
}
