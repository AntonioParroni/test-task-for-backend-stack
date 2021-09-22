using System;

namespace Server.Helper
{
    public static class MySimpleMath
    {
        public static int TakeNDigits(int number, int N)
        {
            number = Math.Abs(number);
            if (number == 0) return number;
            int numberOfDigits = (int)Math.Floor(Math.Log10(number) + 1);
            if (numberOfDigits >= N) return (int)Math.Truncate((number / Math.Pow(10, numberOfDigits - N)));
            return number;
        }
    }
}