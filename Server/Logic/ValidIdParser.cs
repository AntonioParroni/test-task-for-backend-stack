using System;
using System.Linq;

namespace Server.Logic
{
    public static class ValidIdParser
    {
        public static bool Check(int id)
        {
            if (Math.Floor(Math.Log10(id) + 1) > 6) return false; // check for six digits
            int year = MySimpleMath.TakeNDigits(id, 4);
            int month = int.Parse((id % 100).ToString().PadLeft(2, '0'));
            if (!Enumerable.Range(1, 12).Contains(month)) return false; // check for a valid month
            if (!Enumerable.Range(2020, 2021).Contains(year)) return false; // check for a valid year
            
            return true;
        }
    }
}