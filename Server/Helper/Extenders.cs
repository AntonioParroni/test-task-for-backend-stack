using System;

namespace Server.Helper
{
    public static class Extenders {
            public static DateTime ParseRequestTime(this string str){
                DateTime returnTime = new DateTime(int.Parse(str.Substring(0,4)),
                    int.Parse(str.Substring(5,2)),
                    int.Parse(str.Substring(8,2)), 
                    int.Parse(str.Substring(11,2)), 0, 0);
                return returnTime;
            }
        }
}