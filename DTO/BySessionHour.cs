namespace DTO
{
    public class BySessionHour
    {
        public string? date { get; set; }
        public int? hour { get; set; }
        public int? conccurentSessions { get; set; }
        public int? totalTimeForHour { get; set; }
        public int? qumulativeForHour { get; set; }
    }
}