using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Framework
{
    public class DateTimeHelper
    {

        private DateTime dateTime;
        private int offset;

        public DateTimeHelper(DateTime dateTime)
        {
            this.dateTime = dateTime;
            this.offset = GetWeeklyDays();
        }

        public DateTimeHelper()
            : this(DateTime.Now)
        {
        }

        private int GetWeeklyDays()
        {
            int offset = 0;

            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    offset = 0;
                    break;
                case DayOfWeek.Tuesday:
                    offset = -1;
                    break;
                case DayOfWeek.Wednesday:
                    offset = -2;
                    break;
                case DayOfWeek.Thursday:
                    offset = -3;
                    break;
                case DayOfWeek.Friday:
                    offset = -4;
                    break;
                case DayOfWeek.Saturday:
                    offset = -5;
                    break;
                case DayOfWeek.Sunday:
                    offset = -6;
                    break;
            }
            return offset;
        }

        public DateTime WeeklyStart
        {
            get
            {
                var output = dateTime.AddDays(this.offset).ToString("yyyy-MM-dd 00:00:00");
                return DateTime.Parse(output);
            }
        }

        public DateTime WeeklyEnd
        {
            get
            {
                var output = dateTime.AddDays(6 + this.offset).ToString("yyyy-MM-dd 23:59:59");
                return DateTime.Parse(output);
            }
        }

        /// <summary>

        /// unix时间转换为adatetime

        /// </summary>

        /// <param name="timeStamp"></param>

        /// <returns></returns>

        public static DateTime UnixTimeToTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            long lTime = long.Parse(timeStamp + "0000000");

            TimeSpan toNow = new TimeSpan(lTime);

            return dtStart.Add(toNow);

        }



        /// <summary>

        /// datetime转换为aunixtime

        /// </summary>

        /// <param name="time"></param>

        /// <returns></returns>

        public static int ConvertDateTimeInt(System.DateTime time)
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;

        }


    }
}
