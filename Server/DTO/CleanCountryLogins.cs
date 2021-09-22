using System;
#nullable enable
namespace Server.DTO
{
    public class CleanCountryLogin
    {
        public string? country { get; set; }
        public DateTime loginTime { get; set; }
    }
}