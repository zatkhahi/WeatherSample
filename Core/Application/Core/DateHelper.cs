using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Application.Infrastructure
{
    public static class DateHelper
    {
        public static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixMsTime(this long unixMilliSecondsTime)
        {
            return EPOCH.AddMilliseconds(unixMilliSecondsTime);
        }

        public static long ToUnixTimeSeconds(this DateTime utcTime)
        {
            TimeSpan t = utcTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)t.TotalSeconds;
        }
        public static int? PersianDateToInt(this string str)
        {
            var parts = str.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
                return null;
            if (!int.TryParse(parts[0], out int year))
                return null;
            if (!int.TryParse(parts[1], out int month))
                return null;
            if (!int.TryParse(parts[2], out int day))
                return null;
            return year * 10000 + month * 100 + day;
        }

        public static int ToInt(this DateTime date)
        {
            return date.Year * 10000 + date.Month * 100 + date.Day;
        }

        public static int NextMonth(this int yearMonth)
        {
            int year = yearMonth / 100;
            int month = yearMonth % 100;
            int nextMonth = (month + 1) > 12 ? 1 : month + 1;
            int nextYear = (month + 1) > 12 ? year + 1 : year;
            return nextYear * 100 + nextMonth;
        }

        public static int ToPersianDateInt(this DateTime date)
        {
            var pc = new PersianCalendar();
            int year = pc.GetYear(date);
            int month = pc.GetMonth(date);
            int day = pc.GetDayOfMonth(date);
            return year * 10000 + month * 100 + day;
        }

        public static long ToUnixTimeMilliSeconds(this DateTime utcTime)
        {
            TimeSpan t = utcTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)t.TotalMilliseconds;
        }

        public static DateTime ToDateTime(this int persianDate)
        {
            int year = persianDate / 10000;
            int month = (persianDate % 10000) / 100;
            int day = persianDate % 100;
            return new DateTime(year, month, day, new PersianCalendar());            
        }
    }
}
