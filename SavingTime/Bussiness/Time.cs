using System;

namespace SavingTime.Bussiness
{
    public class Time
    {
        private int hour;
        public int Hour {
            get
            {
                return hour;
            }
            set
            {
                if (value < 0 || value > 23) throw new ArgumentException($"Invalid Hour {value}");
                hour = value;
            }
        }

        private int minute;
        public int Minute {
            get
            {
                return minute;
            }
            set
            {
                if (value < 0 || value > 59) throw new ArgumentException($"Invalid Minute {value}");
                minute = value;
            }
        }

        public Time(string time)
        {
            var parts = time.Split(':');
            Hour = Convert.ToInt32(parts[0]);
            Minute = Convert.ToInt32(parts[1]);
        }
        public Time(int hour, int minute)
        {
            Hour = hour;
            Minute = minute;
        }

        public override string ToString()
        {
            return $"{hour.ToString("D2")}:{minute.ToString("D2")}";
        }

        public string ToFraction()
        {
            return $"{hour.ToString("D2")}_{FractionFromMinute()}";
        }

        private string FractionFromMinute()
        {
            if (Minute >= 0 && Minute <= 9) return "1";
            if (Minute >= 10 && Minute <= 19) return "2";
            if (Minute >= 20 && Minute <= 29) return "3";
            if (Minute >= 30 && Minute <= 39) return "4";
            if (Minute >= 40 && Minute <= 49) return "5";
            
            return "6";
        }

        public bool IsGraterThan(Time other)
        {
            if (Hour > other.Hour) return true;

            if (Hour < other.Hour) return false;

            if (Minute > other.Minute) return true;

            return false;
        }

        public void AddMinutes(int minutes)
        {
            var sum = Minute + minutes;

            if (sum <= 59)
            {
                Minute = sum;
                return;
            }

            while (sum > 59)
            {
                var diff = 59 - Minute;
                Hour += 1;
                Minute = 0;
                minutes -= diff + 1;
                sum = Minute + minutes;
            }
        }
    }
}
