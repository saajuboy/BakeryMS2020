using System;

namespace BakeryMS.API.Common.Helpers
{
    public static class Extensions
    {
        public static string DashedDate(this DateTime theDateTime)
        {
            var year = theDateTime.Year;
            var month = theDateTime.Month;
            var day = theDateTime.Day;

            return String.Format("{0:0000}", year) + "-" + String.Format("{0:00}", month) + "-" + String.Format("{0:00}", day);
        }
    }
}