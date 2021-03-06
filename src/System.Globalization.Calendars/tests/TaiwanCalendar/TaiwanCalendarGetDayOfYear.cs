// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using Xunit;

namespace System.Globalization.CalendarsTests
{
    // System.Globalization.TaiwanCalendar.GetDayOfYear(DateTime)
    public class TaiwanCalendarGetDayOfYear
    {
        private readonly int[] _DAYS_PER_MONTHS_IN_LEAP_YEAR = new int[13]
        {
            0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
        };
        private readonly int[] _DAYS_PER_MONTHS_IN_NO_LEAP_YEAR = new int[13]
        {
            0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
        };

        #region PositiveTesting
        // PosTest1: Verify the day is a random Date
        [Fact]
        public void PosTest1()
        {
            System.Globalization.Calendar tc = new TaiwanCalendar();
            Random rand = new Random(-55);
            int year = rand.Next(tc.MinSupportedDateTime.Year, tc.MaxSupportedDateTime.Year - 1911);
            int month = rand.Next(1, 12);
            int day;
            if (tc.IsLeapYear(year))
            {
                day = rand.Next(1, _DAYS_PER_MONTHS_IN_LEAP_YEAR[month] + 1);
            }
            else
            {
                day = rand.Next(1, _DAYS_PER_MONTHS_IN_NO_LEAP_YEAR[month] + 1);
            }

            DateTime dt = new DateTime(year, month, day);
            int actualDay = getDayOfYear(dt);
            Assert.Equal(tc.GetDayOfYear(dt), actualDay);
        }

        // PosTest2: Verify the DateTime is TaiwanCalendar MinSupportedDateTime
        [Fact]
        public void PosTest2()
        {
            System.Globalization.Calendar tc = new TaiwanCalendar();
            DateTime dt = tc.MinSupportedDateTime;
            Assert.Equal(1, tc.GetDayOfYear(dt));
        }

        // PosTest3: Verify the DateTime is TaiwanCalendar MaxSupportDateTime
        [Fact]
        public void PosTest3()
        {
            System.Globalization.Calendar tc = new TaiwanCalendar();
            DateTime dt = tc.MaxSupportedDateTime;
            Assert.Equal(365, tc.GetDayOfYear(dt));
        }

        // PosTest4: Verify the DateTime is leap year
        [Fact]
        public void PosTest4()
        {
            System.Globalization.Calendar tc = new TaiwanCalendar();
            DateTime dt = new DateTime(2000, 12, 31);
            Assert.Equal(366, tc.GetDayOfYear(dt));
        }

        #endregion
        #region Helper Methods
        public int getDayOfYear(DateTime time)
        {
            long TicksPerMillisecond = 10000;
            long TicksPerSecond = TicksPerMillisecond * 1000;
            long TicksPerMinute = TicksPerSecond * 60;
            long TicksPerHour = TicksPerMinute * 60;
            long TicksPerDay = TicksPerHour * 24;
            int DaysPerYear = 365;
            int DaysPer4Years = DaysPerYear * 4 + 1;
            int DaysPer100Years = DaysPer4Years * 25 - 1;
            int DaysPer400Years = DaysPer100Years * 4 + 1;
            // n = number of days since 1/1/0001
            int n = (int)(time.Ticks / TicksPerDay);
            // y400 = number of whole 400-year periods since 1/1/0001
            int y400 = n / DaysPer400Years;
            // n = day number within 400-year period
            n -= y400 * DaysPer400Years;
            // y100 = number of whole 100-year periods within 400-year period
            int y100 = n / DaysPer100Years;
            // Last 100-year period has an extra day, so decrement result if 4
            if (y100 == 4)
                y100 = 3;
            // n = day number within 100-year period
            n -= y100 * DaysPer100Years;
            // y4 = number of whole 4-year periods within 100-year period
            int y4 = n / DaysPer4Years;
            // n = day number within 4-year period
            n -= y4 * DaysPer4Years;
            // y1 = number of whole years within 4-year period
            int y1 = n / DaysPerYear;
            // Last year has an extra day, so decrement result if 4
            if (y1 == 4)
                y1 = 3;
            // n = day number within year
            n -= y1 * DaysPerYear;
            // If day-of-year was requested, return it
            return (n + 1);
        }
        #endregion
    }
}